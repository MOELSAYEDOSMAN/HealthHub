from flask import Flask, request, jsonify,make_response #make_response 
##---------------------------------##
from flask_cors import CORS
##---------------------------------##

from test_functions import check_interaction, get_interaction_name


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
    

if __name__ == '__main__':
    app.run(debug=True)
