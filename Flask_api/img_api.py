from flask import Flask, request, jsonify,make_response #make_response 
##---------------------------------##
from flask_cors import CORS
##---------------------------------##
import os
import numpy as np
from tensorflow.keras.preprocessing import image
from tensorflow.keras.applications.vgg16 import preprocess_input
#import pickle
#from test_functions import check_interaction, get_interaction_name


app = Flask(__name__)
##---------------------------------##
CORS(app)#enable Send From Api(Backend Asp)
##---------------------------------##

#Change Name
# @app.route('/CheckInteraction', methods=['POST'])
# def predict_interaction():
#     data = request.get_json()
#     drug_1 = data.get('drug_1')
#     drug_2 = data.get('drug_2')
    
#     interaction_result = check_interaction(drug_1, drug_2)
#     ##---------------------------------##
#     if(interaction_result==0):
#         return jsonify({
#         'InteractionResult': False,
#         'InteractionNameIndex':-1
#     })
#     else:
#         InteractionName=get_interaction_name(drug_1,drug_2)
#         return jsonify({
#         'InteractionResult': True,
#         'InteractionNameIndex':InteractionName
#     })
    ##--------------------------------##
    
@app.route('/predict', methods=['POST'])
def predict():
    print("Statrt")
    if 'file' not in request.files:
        return jsonify('No file part')

    print("FoundFile")
    file = request.files['file']

    if file.filename == '':
        return jsonify('No selected file')
    print("Get File")
    print(file.filename)
    if file:
        filepath = os.path.join('temp', file.filename)
        file.save(filepath)

        # Preprocess the image
        transformer = ImageTransformer()
        img_processed = transformer.transform(filepath)
     
    
        # Predict
 
        # Clean up
        os.remove(filepath)

        result = "Done"
        #"not fractured" if y_pred == 1 else "fractured"
        return jsonify(result)
    

if __name__ == '__main__':
    app.run(debug=True)





class ImageTransformer:
    def __init__(self, target_size=(224, 224)):
        self.target_size = target_size

    def transform(self, img_path):
        img = image.load_img(img_path, target_size=self.target_size)
        img_array = image.img_to_array(img)
        img_array = np.expand_dims(img_array, axis=0)
        img_array = preprocess_input(img_array)
        return img_array