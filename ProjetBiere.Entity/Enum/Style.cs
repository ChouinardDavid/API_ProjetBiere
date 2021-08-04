using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ProjetBiere.Entity
{
    public enum Style
    {
        IPA,

        [EnumMember(Value = "Session IPA")]
        SessionIPA,

        Blonde,

        Lagger,

        Blanche,
    }
}