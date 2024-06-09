import time
from data_processing import BipartiteData, get_bipartite_graph,get_mol_edge_list_and_feat_mtx

import torch
from rdkit import Chem
from torch_geometric.data import Data, Batch
import warnings

def create_input_data(drug_1, drug_2, type):
    b = drug_1
    c = drug_2
    df_drugs_smiles = {0: b, 1: c}

    drug_id_mol_graph_tup = [(id, Chem.MolFromSmiles(smiles.strip())) for id, smiles in df_drugs_smiles.items()]

    drug_to_mol_graph = {id: Chem.MolFromSmiles(smiles.strip()) for id, smiles in df_drugs_smiles.items()}
    MOL_EDGE_LIST_FEAT_MTX = {drug_id: get_mol_edge_list_and_feat_mtx(mol)
                              for drug_id, mol in drug_id_mol_graph_tup}
    MOL_EDGE_LIST_FEAT_MTX = {drug_id: mol for drug_id, mol in MOL_EDGE_LIST_FEAT_MTX.items() if mol is not None}

    data_0 = create_graph_data(0, MOL_EDGE_LIST_FEAT_MTX)
    data_0_0 = [data_0]
    data_1 = create_graph_data(1,MOL_EDGE_LIST_FEAT_MTX)
    data_1_1 = [data_1]

    h_graph = drug_to_mol_graph[0]
    t_graph = drug_to_mol_graph[1]
    s = create_b_graph(get_bipartite_graph(h_graph, t_graph), data_0.x, data_1.x)
    s_s = [s]
    data_0 = Batch.from_data_list(data_0_0)
    data_1 = Batch.from_data_list(data_1_1)
    s = Batch.from_data_list(s_s)
    pos = [type]
    pos_rels = torch.LongTensor(pos).unsqueeze(0)
    pos_tri = (data_0, data_1, pos_rels, s)
    return pos_tri


def create_graph_data(id, MOL_EDGE_LIST_FEAT_MTX):
    edge_index = MOL_EDGE_LIST_FEAT_MTX[id][0]
    n_features = MOL_EDGE_LIST_FEAT_MTX[id][1]
    return Data(x=n_features, edge_index=edge_index)


def create_b_graph(edge_index, x_s, x_t):
    return BipartiteData(edge_index, x_s, x_t)


path="D:\Project\Graduation projects\FindalProject\HealthHup\Flask_api\ddi.pkl" #the model path
model=torch.load(path,map_location=torch.device('cpu'))

def check_interaction(drug_1,drug_2):

        drug_1 = drug_1
        drug_2 = drug_2
        start_time = time.time()
        break_outer_loop = False
        for i in range(0, 86):
            pos = create_input_data(drug_1, drug_2, i)

            model.eval()
            pod = model(pos)
            if torch.sigmoid(pod.detach())>0.50 :
                return 1
        return 0
def get_interaction_name (drug_1,drug_2):
    drug_1 = drug_1
    drug_2 = drug_2
    start_time = time.time()
    break_outer_loop = False
    for i in range(0, 86):
        pos = create_input_data(drug_1, drug_2, i)

        model.eval()
        pod = model(pos)
        if torch.sigmoid(pod.detach()) > 0.50:
            return i
    return None










