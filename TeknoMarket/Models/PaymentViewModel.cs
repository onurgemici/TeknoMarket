using System.ComponentModel.DataAnnotations;

namespace TeknoMarket.Models;

public class PaymentViewModel
{
        [Display(Name = "Kredi Kartı Numarası")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public string CreditCardNumber { get; set; } = string.Empty;
        [Display(Name = "CVV/CVC")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public int CVV { get; set; }
        [Display(Name = "Ad Soyad")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public string FullName { get; set; } = string.Empty;
        [Display(Name = "Adres")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public Guid AddressId { get; set; }

        [Display(Name = "Adres Adı")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public string AddressName { get; set; } = string.Empty;
        [Display(Name = "Adres")]
        [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
        public string Address { get; set; } = string.Empty;
}
