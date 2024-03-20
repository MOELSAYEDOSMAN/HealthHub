import math
import datetime

import torch
from torch import nn
import torch.nn.functional as F

from torch_geometric.nn import GCNConv, SAGPooling, global_add_pool, GATConv


class CoAttentionLayer(nn.Module):
    def __init__(self, n_features):
        super().__init__()
        self.n_features = n_features
        self.w_q = nn.Parameter(torch.zeros(n_features, n_features // 2))
        self.w_k = nn.Parameter(torch.zeros(n_features, n_features // 2))
        self.bias = nn.Parameter(torch.zeros(n_features // 2))
        self.a = nn.Parameter(torch.zeros(n_features // 2))

        nn.init.xavier_uniform_(self.w_q)
        nn.init.xavier_uniform_(self.w_k)
        nn.init.xavier_uniform_(self.bias.view(*self.bias.shape, -1))  # Reshape bias for broadcasting
        nn.init.xavier_uniform_(self.a.view(*self.a.shape, -1))     # Reshape a for broadcasting

    def forward(self, receiver, attendant):
        keys = receiver @ self.w_k
        queries = attendant @ self.w_q
        # values = receiver @ self.w_v
        values = receiver

        e_activations = queries.unsqueeze(-3) + keys.unsqueeze(-2) + self.bias
        e_scores = torch.tanh(e_activations) @ self.a
        # e_scores = e_activations @ self.a
        attentions = e_scores
        return attentions

class RESCAL(nn.Module):
    def __init__(self, n_rels, n_features):
        super().__init__()
        self.n_rels = n_rels
        self.n_features = n_features
        self.rel_emb = nn.Embedding(self.n_rels, n_features * n_features)
        nn.init.xavier_uniform_(self.rel_emb.weight)

    def forward(self, heads, tails, rels, alpha_scores):
        rels = self.rel_emb(rels)

        rels = F.normalize(rels, dim=-1)
        heads = F.normalize(heads, dim=-1)
        tails = F.normalize(tails, dim=-1)

        rels = rels.view(-1, self.n_features, self.n_features)
        # print(heads.size(),rels.size(),tails.size())
        scores = heads @ rels @ tails.transpose(-2, -1)

        if alpha_scores is not None:
            scores = alpha_scores * scores
        scores = scores.sum(dim=(-2, -1))

        return scores

    def __repr__(self):
        return f"{self.__class__.__name__}({self.n_rels}, {self.rel_emb.weight.shape})"


# intra rep
class IntraGraphAttention(nn.Module):
    def __init__(self, input_dim):
        super().__init__()
        self.input_dim = input_dim
        self.intra = GATConv(input_dim, 32, 2)

    def forward(self, data):
        input_feature, edge_index = data.x, data.edge_index
        input_feature = F.elu(input_feature)
        intra_rep = self.intra(input_feature, edge_index)
        return intra_rep


# inter rep
class InterGraphAttention(nn.Module):
    def __init__(self, input_dim):
        super().__init__()
        self.input_dim = input_dim
        self.inter = GATConv((input_dim, input_dim), 32, 2)

    def forward(self, h_data, t_data, b_graph):
        edge_index = b_graph.edge_index
        h_input = F.elu(h_data.x)
        t_input = F.elu(t_data.x)
        t_rep = self.inter((h_input, t_input), edge_index)
        h_rep = self.inter((t_input, h_input), edge_index[[1, 0]])
        return h_rep, t_rep


"""import torch
from torch_geometric.data import Data

# Instantiate CoAttentionLayer
n_features = 64  # Example value
co_attention_layer = CoAttentionLayer(n_features)

# Instantiate RESCAL
n_rels = 10  # Example value
n_features = 64  # Example value
rescal = RESCAL(n_rels, n_features)

# Instantiate IntraGraphAttention
input_dim = 64  # Example value
intra_graph_attention = IntraGraphAttention(input_dim)

# Instantiate InterGraphAttention
inter_graph_attention = InterGraphAttention(input_dim)

# Create sample input tensors (you need to replace these with your actual data)
receiver = torch.randn(32, 64)  # Example: 32 samples, each with 64 features
attendant = torch.randn(32, 64)  # Example: 32 samples, each with 64 features
heads = torch.randn(32, 64)  # Example: 32 samples, each with 64 features
tails = torch.randn(32, 64)  # Example: 32 samples, each with 64 features
rels = torch.randint(0, n_rels, (32,))  # Example: 32 samples, each with a relation index
alpha_scores = torch.randn(32)  # Example: 32 alpha scores

# Test CoAttentionLayer
attentions = co_attention_layer(receiver, attendant)
print("CoAttentionLayer output shape:", attentions.shape)

# Test RESCAL
scores = rescal(heads, tails, rels, alpha_scores)
print("RESCAL output shape:", scores.shape)

# Test IntraGraphAttention
data = Data(x=torch.randn(64, 64), edge_index=torch.tensor([[0, 1], [1, 0]]))  # Example data
intra_rep = intra_graph_attention(data)
print("IntraGraphAttention output shape:", intra_rep.shape)

# Test InterGraphAttention
h_rep, t_rep = inter_graph_attention(data, data, data)
print("InterGraphAttention h_rep shape:", h_rep.shape)
print("InterGraphAttention t_rep shape:", t_rep.shape)"""
