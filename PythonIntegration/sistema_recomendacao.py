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
    return similaridade[0:30]

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
    
    
def loadData(path=r'C:\Users\Usuario\Desktop\Programacao\Aulas\Python\ml-100k'):
        filmes = {}
        for linha in open(path + '/u.item'):
            id, titulo  = linha.split('|')[0:2]
            filmes[id] = titulo
        base = {}
        for linha in open(path + '/u.data'):
            user, id_filme, nota = linha.split('\t')[0:3]
            base.setdefault(user,{})
            base[user][filmes[id_filme]] = float(nota)
        return base
                
                
def calculaItensSimilares(base):
    result = {}
    for item in base:
        sims = getSimilares(base,item)
        result[item] = sims
    return result

def getRecomendacoesItens(baseUsuario, similaridadeItens, usuario):
    notasUsuario = baseUsuario[usuario]
    notas = {}
    totalSimilaridade = {}
    for item, nota in notasUsuario.items():
        for similaridade, item2 in similaridadeItens[item]:
            if item2 in notasUsuario.items():
                continue
            notas.setdefault(item2, 0)
            notas[item2] += similaridade * nota
            totalSimilaridade.setdefault(item2, 0)
            totalSimilaridade[item2] += similaridade
            
    ranking = [ (score/totalSimilaridade[item], item) for item, score in notas.items()]
    ranking.sort()
    ranking.reverse()
    return ranking

