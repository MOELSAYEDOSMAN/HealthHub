from test_functions import  check_interaction
import warnings
import time

def get_drug_interactions():
    num_interactions = int(input("Enter the number of interactions you want to check: "))
    drug_pairs = []

    for i in range(num_interactions):
        drug1 = input(f"Enter the first drug for interaction {i+1}: ")
        drug2 = input(f"Enter the second drug for interaction {i+1}: ")
        drug_pairs.append((drug1, drug2))

    return drug_pairs

# Example usage:
drugs = get_drug_interactions()
warnings.filterwarnings('ignore')

"""""
drug smiles if want to test model 
1-C[N@+]1(CC2CC2)CC[C@]23[C@H]4OC5=C(O)C=CC(C[C@@H]1[C@]2(O)CCC4=O)=C35"
2-"CCCCCCCCCCCCCCCC(O)=O"
3-"CC(C)COC1=C(C=C(C=C1)C1=NC(C)=C(S1)C(O)=O)C#N"
4-"CC(C)NCC1CCC2=CC(CO)=C(C=C2N1)[N+]([O-])=O"
5-"OC1=CC(=O)NC=C1Cl
"""""


print(f"the number of relation is {len(drugs)}")
print("-----------------------------------------")
print("check interactions...")

start_time = time.time()
for i in drugs:
    if check_interaction(i[0],i[1])==1:

        print(f"the interaction between ({i[0],i[1]}) is 1 ")

        print("--------------------------------------")
    else:

        print(f"the interaction between ({i[0], i[1]}) is 0")
        print("--------------------------------------")

end_time = time.time()
elapsed_time = end_time - start_time
print("time :", elapsed_time, "seconds")

