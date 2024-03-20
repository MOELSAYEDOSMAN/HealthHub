import torch
from torch_geometric.data import Data, Batch
from rdkit import Chem

import numpy as np


AVAILABLE_ATOM_SYMBOLS=['Bi', 'Se', 'Zn', 'C', 'F', 'As', 'Ga', 'Ra', 'I', 'Ca', 'Br', 'H', 'P', 'Au', 'O', 'Li', 'Al', 'Cr', 'Ti', 'Cu', 'Cl', 'Ag', 'Sb', 'Gd', 'K', 'Hg', 'Co', 'N', 'Na', 'Mg', 'B', 'S', 'La', 'Fe', 'Sr', 'Tc', 'Si', 'Pt']
AVAILABLE_ATOM_DEGREES= [0, 1, 2, 3, 4, 5, 6]
AVAILABLE_ATOM_TOTAL_HS=[0, 1, 2, 3, 4]
AVAILABLE_ATOM_VALENCE=[0 ,1, 2 ,3 ,4 ,5 ,6 ,7 ,8 ,9]
##########################################
class BipartiteData(Data):
    def __init__(self, edge_index=None, x_s=None, x_t=None):
        super().__init__()
        self.edge_index = edge_index
        self.x_s = x_s
        self.x_t = x_t

    def __inc__(self, key, value, *args, **kwargs):
        if key == 'edge_index':
            return torch.tensor([[self.x_s.size(0)], [self.x_t.size(0)]])
        else:
            return super().__inc__(key, value, *args, **kwargs)


def get_bipartite_graph(mol_graph_1, mol_graph_2):
    x1 = np.arange(0, len(mol_graph_1.GetAtoms()))
    x2 = np.arange(0, len(mol_graph_2.GetAtoms()))
    edge_list = torch.LongTensor(np.meshgrid(x1, x2))
    edge_list = torch.stack([edge_list[0].reshape(-1), edge_list[1].reshape(-1)])
    return edge_list
#########################################
def one_of_k_encoding_unk(x, allowable_set):
    if x not in allowable_set:
        x = allowable_set[-1]
    return list(map(lambda s: x == s, allowable_set))
def atom_features(atom,
                explicit_H=True,
                use_chirality=False):

    results = one_of_k_encoding_unk(
        atom.GetSymbol(),
        ['C','N','O', 'S','F','Si','P', 'Cl','Br','Mg','Na','Ca','Fe','As','Al','I','B','V','K','Tl',
            'Yb','Sb','Sn','Ag','Pd','Co','Se','Ti','Zn','H', 'Li','Ge','Cu','Au','Ni','Cd','In',
            'Mn','Zr','Cr','Pt','Hg','Pb','Unknown'
        ]) + [atom.GetDegree()/10, atom.GetImplicitValence(),
                atom.GetFormalCharge(), atom.GetNumRadicalElectrons()] + \
                one_of_k_encoding_unk(atom.GetHybridization(), [
                Chem.rdchem.HybridizationType.SP, Chem.rdchem.HybridizationType.SP2,
                Chem.rdchem.HybridizationType.SP3, Chem.rdchem.HybridizationType.
                                    SP3D, Chem.rdchem.HybridizationType.SP3D2
                ]) + [atom.GetIsAromatic()]
    # In case of explicit hydrogen(QM8, QM9), avoid calling `GetTotalNumHs`
    if explicit_H:
        results = results + [atom.GetTotalNumHs()]

    if use_chirality:
        try:
            results = results + one_of_k_encoding_unk(
            atom.GetProp('_CIPCode'),
            ['R', 'S']) + [atom.HasProp('_ChiralityPossible')]
        except:
            results = results + [False, False
                            ] + [atom.HasProp('_ChiralityPossible')]

    results = np.array(results).astype(np.float32)

    return torch.from_numpy(results)
def get_mol_edge_list_and_feat_mtx(mol_graph):
    n_features = [(atom.GetIdx(), atom_features(atom)) for atom in mol_graph.GetAtoms()]
    n_features.sort() # to make sure that the feature matrix is aligned according to the idx of the atom
    _, n_features = zip(*n_features)
    n_features = torch.stack(n_features)

    edge_list = torch.LongTensor([(b.GetBeginAtomIdx(), b.GetEndAtomIdx()) for b in mol_graph.GetBonds()])
    undirected_edge_list = torch.cat([edge_list, edge_list[:, [1, 0]]], dim=0) if len(edge_list) else edge_list
    return undirected_edge_list.T, n_features





