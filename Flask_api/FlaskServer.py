from flask import Flask, request, jsonify,make_response #make_response 
##---------------------------------##
from flask_cors import CORS
##---------------------------------##
from test_functions import check_interaction, get_interaction_name
#Xray
import os
import numpy as np
from tensorflow.keras.preprocessing import image
from tensorflow.keras.applications.vgg16 import preprocess_input
import pickle

app = Flask(__name__)
##---------------------------------##
CORS(app)#enable Send From Api(Backend Asp)
##---------------------------------##

#Change Name
@app.route('/CheckInteraction', methods=['POST'])
def predict_interaction():
    data = request.get_json()
    drug_1 = data.get('drug_1')
    drug_2 = data.get('drug_2')
    
    interaction_result = check_interaction(drug_1, drug_2)
    ##---------------------------------##
    if(interaction_result==0):
        return jsonify({
        'InteractionResult': False,
        'InteractionNameIndex':-1
    })
    else:
        InteractionName=get_interaction_name(drug_1,drug_2)
        return jsonify({
        'InteractionResult': True,
        'InteractionNameIndex':InteractionName
    })
##--------------------------------##
#Xray#
# Load the model
with open('F:/xray/model.pkl', 'rb') as file:
    f_model = pickle.load(file)
class ImageTransformer:
    def __init__(self, target_size=(224, 224)):
        self.target_size = target_size

    def transform(self, img_path):
        img = image.load_img(img_path, target_size=self.target_size)
        img_array = image.img_to_array(img)
        img_array = np.expand_dims(img_array, axis=0)
        img_array = preprocess_input(img_array)
        return img_array
@app.route('/predict', methods=['POST'])
def predict():
    if 'file' not in request.files:
        return jsonify('No file part')

    file = request.files['file']

    if file.filename == '':
        return jsonify('No selected file')

    if file:
        filepath = os.path.join('temp', file.filename)
        file.save(filepath)

        # Preprocess the image
        transformer = ImageTransformer()
        img_processed = transformer.transform(filepath)

        # Predict
        pred = f_model.predict(img_processed)
        y_pred = np.where(pred >= 0.5, 1, 0)
        y_pred = np.ravel(y_pred)[0]

        # Clean up
        os.remove(filepath)

        result = "not fractured" if y_pred == 1 else "fractured"
        return jsonify(result)
    
    
    
    
    #end Xray
if __name__ == '__main__':
    app.run(debug=True)