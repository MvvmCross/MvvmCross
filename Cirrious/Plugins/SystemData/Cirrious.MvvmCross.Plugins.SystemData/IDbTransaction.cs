using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cirrious.MvvmCross.Plugins.SystemData
{
    public interface IDbTransaction : IDisposable
    {
        // Résumé :
        //     Spécifie l'objet Connection à associer à la transaction.
        //
        // Retourne :
        //     Objet Connection à associer à la transaction.
        IDbConnection Connection { get; }
        // Résumé :
        //     Valide la transaction de base de données.
        //
        // Exceptions :
        //   System.Exception:
        //     Une erreur s'est produite lors de la tentative de validation de la transaction.
        //
        //   System.InvalidOperationException:
        //     La transaction a déjà été validée ou restaurée. ou La connexion est interrompue.
        void Commit();
        //
        // Résumé :
        //     Restaure une transaction à partir d'un état d'attente.
        //
        // Exceptions :
        //   System.Exception:
        //     Une erreur s'est produite lors de la tentative de validation de la transaction.
        //
        //   System.InvalidOperationException:
        //     La transaction a déjà été validée ou restaurée. ou La connexion est interrompue.
        void Rollback();
    }
}
