using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cirrious.MvvmCross.Plugins.SystemData
{
    public interface IDbConnection
    {

        object InnerConnection { get; }
        // Résumé :
        //     Commence une transaction de base de données.
        //
        // Retourne :
        //     Objet représentant la nouvelle transaction.
        IDbTransaction BeginTransaction();
        //
        // Résumé :
        //     Ferme la connexion à la base de données.
        void Close();
        //
        // Résumé :
        //     Crée et retourne un objet Command associé à la connexion.
        //
        // Retourne :
        //     Objet Command associé à la connexion.
        IDbCommand CreateCommand();
        //
        // Résumé :
        //     Ouvre une connexion de base de données à l'aide des paramètres spécifiés
        //     par la propriété ConnectionString de l'objet Connection propre au fournisseur.
        void Open();
    }
}
