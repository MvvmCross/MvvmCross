using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cirrious.MvvmCross.Plugins.SystemData
{
    public interface IDataParameterCollection : IList, ICollection, IEnumerable
    {
        // Résumé :
        //     Obtient ou définit le paramètre à l'index spécifié.
        //
        // Paramètres :
        //   parameterName:
        //     Nom du paramètre à récupérer.
        //
        // Retourne :
        //     System.Object à l'index spécifié.
        object this[string parameterName] { get; set; }

        // Résumé :
        //     Obtient une valeur indiquant si un paramètre de la collection porte le nom
        //     spécifié.
        //
        // Paramètres :
        //   parameterName:
        //     Nom du paramètre.
        //
        // Retourne :
        //     true si la collection contient le paramètre ; sinon, false.
        bool Contains(string parameterName);
        //
        // Résumé :
        //     Obtient l'emplacement de System.Data.IDataParameter dans la collection.
        //
        // Paramètres :
        //   parameterName:
        //     Nom du paramètre.
        //
        // Retourne :
        //     Emplacement de base zéro de System.Data.IDataParameter dans la collection.
        int IndexOf(string parameterName);
        //
        // Résumé :
        //     Supprime System.Data.IDataParameter de la collection.
        //
        // Paramètres :
        //   parameterName:
        //     Nom du paramètre.
        void RemoveAt(string parameterName);

        int Add(string parameterName, object value);
    }
}
