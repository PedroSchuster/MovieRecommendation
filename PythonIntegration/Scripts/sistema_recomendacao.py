import pandas as pd

def euclidiana(base, user1, user2):
    si = {}
    for item in base[user1]:
        if item in base[user2]:
            si[item] = 1
    if len(si) == 0:
        return 0
    
    soma = sum(pow(base[user1][item] - base[user2][item],2) 
            for item in base[user1] if item in base[user2])

    return 1/((soma)**0.5 + 1)


def getSimilares(base,user):
    similaridade = [(euclidiana(base, user, outro), outro) for outro in base if outro != user]
    similaridade.sort()
    similaridade.reverse()
    return similaridade

def calculaItensSimilares(base):
    result = {}
    for item in base:
        sims = getSimilares(base,item)
        result[item] = sims
    return result

def getRecomendacoesItens():
    baseUsuario = pd.read_csv(r"C:\Users\Usuario\Desktop\Programacao\Aulas\Python\PythonIntegration\PythonIntegration\Data\userrating.csv")
    path_sim = r"C:\Users\Usuario\Desktop\Programacao\Aulas\Python\PythonIntegration\PythonIntegration\Data\similares.csv"
    
    similaridade_itens = pd.read_csv(path_sim, index_col=0)
    similaridade_itens_dict = similaridade_itens.to_dict("split")
    similaridade_itens_dict = dict(zip(similaridade_itens_dict["index"],similaridade_itens_dict["data"]))

    notasUsuario = baseUsuario.values
    notas = {}
    totalSimilaridade = {}
    
    movies_dict = loadMoviesData()
    
    for item, nota in notasUsuario:
        if item in similaridade_itens_dict.keys():
            for sim_tuple in pd.array(similaridade_itens_dict[item], dtype=list):
                sim_tuple = tuple(map(str,sim_tuple.replace('(', '').replace(')', '').split(',')))
                if str(item) + ".0" == sim_tuple[1]:
                    continue
                
                genres_origin = movies_dict[int(item)]["genres"]
                genres_comp = movies_dict[int(sim_tuple[1].replace(".0",''))]["genres"]
                
                count_genres_sim = len([item for item in genres_comp if item in genres_origin])
                count_total_genres = len(genres_comp)
                
                sim_adjustment = (float(sim_tuple[0]) * (count_genres_sim + 1)) / (count_total_genres + 1)
                
                notas.setdefault(sim_tuple[1], 0)
                notas[sim_tuple[1]] += sim_adjustment * nota 
                
                totalSimilaridade.setdefault(sim_tuple[1], 0)
                totalSimilaridade[sim_tuple[1]] += sim_adjustment 
                 
    ranking = []
    for item, score in notas.items():
        if not totalSimilaridade[item] or totalSimilaridade[item] == 0:
            totalSimilaridade[item] = 1
            score = 0
        ranking.append((item,score/totalSimilaridade[item]))
    ranking.sort(key=lambda tup : tup[1], reverse=True)
    return ranking


def loadMoviesData():
    movies = pd.read_csv(r"C:\Users\Usuario\Desktop\Programacao\Aulas\Python\PythonIntegration\PythonIntegration\Data\movies2.csv")
    movies_dict = {}
    for linha in movies.values:
        id, title, genres  = linha
        genres_splited = genres.split('|')
        movies_dict[id] = {"title" : title, "genres" : genres_splited}
    return movies_dict

result = getRecomendacoesItens()
print(result)