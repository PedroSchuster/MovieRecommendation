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

<<<<<<< HEAD
=======
def getRecomendacoes(base, user):
    totais = {}
    somaSim = {}
    for outro in base:
        if outro == user:
            continue
        similaridade = euclidiana(base, user, outro)
    
        if similaridade <= 0:
            continue

        for item in base[outro]:
            if item not in base[user]:
                totais.setdefault(item,0)
                totais[item] += base[outro][item] * similaridade
                somaSim.setdefault(item,0)
                somaSim[item] += similaridade
    rankings = [(total / somaSim[item], item) for item, total  in totais.items()]
    rankings.sort()
    rankings.reverse()
    return rankings[0:30]
    
                
>>>>>>> bb26de2b8705702aa55c363d0dde54c269150655
def calculaItensSimilares(base):
    result = {}
    for item in base:
        sims = getSimilares(base,item)
        result[item] = sims
    return result

def getRecomendacoesItens():
    baseUsuario = pd.read_csv(r"C:\Users\Usuario\Desktop\Programacao\Aulas\Python\PythonIntegration\PythonIntegration\userrating.csv")
    path_sim = r"C:\Users\Usuario\Desktop\Programacao\Aulas\Python\PythonIntegration\PythonIntegration\similares.csv"
    
    similaridade_itens = pd.read_csv(path_sim, index_col=0)
    similaridade_itens_dict = similaridade_itens.to_dict("split")
    similaridade_itens_dict = dict(zip(similaridade_itens_dict["index"],similaridade_itens_dict["data"]))

    notasUsuario = baseUsuario.values
    notas = {}
    totalSimilaridade = {}
<<<<<<< HEAD
    
    movies_dict = loadMoviesData()
    
=======
>>>>>>> bb26de2b8705702aa55c363d0dde54c269150655
    for item, nota in notasUsuario:
        if item in similaridade_itens_dict.keys():
            for sim_tuple in pd.array(similaridade_itens_dict[item], dtype=list):
                sim_tuple = tuple(map(str,sim_tuple.replace('(', '').replace(')', '').split(',')))
                if str(item) + ".0" == sim_tuple[1]:
                    continue
<<<<<<< HEAD
                
                genres_origin = movies_dict[int(item)]["genres"]
                genres_comp = movies_dict[int(sim_tuple[1].replace(".0",''))]["genres"]
                
                count_genres = len([item for item in genres_comp if item in genres_origin])
                
                notas.setdefault(sim_tuple[1], 0)
                notas[sim_tuple[1]] += float(sim_tuple[0]) * nota * count_genres
                totalSimilaridade.setdefault(sim_tuple[1], 0)
                totalSimilaridade[sim_tuple[1]] += float(sim_tuple[0]) + count_genres
=======
                notas.setdefault(sim_tuple[1], 0)
                notas[sim_tuple[1]] += float(sim_tuple[0]) * nota
                totalSimilaridade.setdefault(sim_tuple[1], 0)
                totalSimilaridade[sim_tuple[1]] += float(sim_tuple[0])
>>>>>>> bb26de2b8705702aa55c363d0dde54c269150655
                
    ranking = []
    for item, score in notas.items():
        if not totalSimilaridade[item] or totalSimilaridade[item] == 0:
            totalSimilaridade[item] = score * 2
        ranking.append((item,score/totalSimilaridade[item]))
<<<<<<< HEAD
    ranking.sort(key=lambda tup : tup[1], reverse=True)
    return ranking


def loadMoviesData():
    movies = pd.read_csv(r"C:\Users\Usuario\Desktop\Programacao\Aulas\Python\PythonIntegration\PythonIntegration\movies2.csv")
    movies_dict = {}
    for linha in movies.values:
        id, title, genres  = linha
        genres_splited = genres.split('|')
        movies_dict[id] = {"title" : title, "genres" : genres_splited}
=======
    return ranking


def loadData(movies_dict, csv_ratings):
        base = {}
        for linha in csv_ratings.values:
            user, id_filme, nota = linha
            if (id_filme in movies_dict.keys()):
                base.setdefault(id_filme,{})
                base[int(id_filme)][int(user)] = float(nota)
        return base

def loadMoviesData():
    movies = pd.read_csv("movies2.csv")
    movies_dict = {}
    for linha in movies.values:
        id, title, genres  = linha
        movies_dict[id] = title, genres
>>>>>>> bb26de2b8705702aa55c363d0dde54c269150655
    return movies_dict

result = getRecomendacoesItens()