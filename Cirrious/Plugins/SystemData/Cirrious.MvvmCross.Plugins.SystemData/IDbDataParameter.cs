
namespace Cirrious.MvvmCross.Plugins.SystemData
{
    public interface IDbDataParameter : IDataParameter
    {
        // Résumé :
        //     Indique la précision de paramètres numériques.
        //
        // Retourne :
        //     Nombre maximal de chiffres utilisés pour représenter la propriété Value de
        //     l'objet Parameter d'un fournisseur de données. La valeur par défaut est 0,
        //     indiquant qu'un fournisseur de données définit la précision de Value.
        byte Precision { get; set; }
        //
        // Résumé :
        //     Indique l'échelle de paramètres numériques.
        //
        // Retourne :
        //     Nombre de décimales selon lequel System.Data.OleDb.OleDbParameter.Value est
        //     résolu. La valeur par défaut est 0.
        byte Scale { get; set; }
        //
        // Résumé :
        //     Taille du paramètre.
        //
        // Retourne :
        //     Taille maximale, en octets, des données figurant dans la colonne. La valeur
        //     par défaut est déduite de la valeur du paramètre.
        int Size { get; set; }
    }
}
