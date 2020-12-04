using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fitnezz.Web.Web.ViewModels
{
    public enum Goals
    {
        [Display(Name = "Mini cut")]
        MiniCut,
        [Display(Name = "Lose weight")]
        LoseWeight,
        Maintain,
        [Display(Name = "Gain weight")]
        GainWeight,
    }
}