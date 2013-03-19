using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cirrious.MvvmCross.Plugins.SystemData
{
    public interface IDataParameter
    {
        // Résumé :
        //     Obtient ou définit le System.Data.DbType du paramètre.
        //
        // Retourne :
        //     Une des valeurs de System.Data.DbType. La valeur par défaut est System.Data.DbType.String.
        //
        // Exceptions :
        //   System.ArgumentOutOfRangeException:
        //     La valeur de la propriété n'est pas un System.Data.DbType valide.
        //DbType DbType { get; set; }
        //
        // Résumé :
        //     Obtient une valeur indiquant si le paramètre accepte les valeurs null.
        //
        // Retourne :
        //     true si les valeurs null sont acceptées ; sinon, false. La valeur par défaut
        //     est false.
        bool IsNullable { get; }
        //
        // Résumé :
        //     Obtient ou définit le nom de System.Data.IDataParameter.
        //
        // Retourne :
        //     Nom de l'élément System.Data.IDataParameter. La valeur par défaut est une
        //     chaîne vide.
        string ParameterName { get; set; }
        //
        // Résumé :
        //     Obtient ou définit le nom de la colonne source qui est mappée à System.Data.DataSet
        //     et utilisée pour charger et retourner System.Data.IDataParameter.Value.
        //
        // Retourne :
        //     Nom de la colonne source mappée à System.Data.DataSet. La valeur par défaut
        //     est une chaîne vide.
        string SourceColumn { get; set; }
        //
        // Résumé :
        //     Obtient ou définit la valeur du paramètre.
        //
        // Retourne :
        //     System.Object correspondant à la valeur du paramètre. La valeur par défaut
        //     est null.
        object Value { get; set; }
    }
}
