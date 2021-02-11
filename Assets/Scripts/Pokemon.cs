using System;
using System.Collections.Generic;

[Serializable]
public class Pokemon
{
    public string height;
    public string weight;
    public List<FormsList> forms;
    public List<TypesList> types;

    public override string ToString()
    {
        return "Pokemon: " + forms[0].name + "\n" +
            "Altura: " + height + "\n" +
            "Tipo: " + types[0].type.name + "\n" +
            "Peso: " + weight
        ;
    }
}

[Serializable]
public class FormsList
{
    public string name;
}

[Serializable]
public class TypesList
{
    public TypePokemon type;
}

[Serializable]
public class TypePokemon
{
    public string name;
    public string url;
}
