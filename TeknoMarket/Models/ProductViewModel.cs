using System.ComponentModel.DataAnnotations;

namespace TeknoMarket.Models;

public class ProductViewModel
{
    [Display(Name = "Aktif")]
    public bool Enabled { get; set; }

    [Display(Name = "Ürün Adı")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    [MaxLength(400, ErrorMessage = "{0} alanı en fazla {1} karakter olmalıdır!")]
    public string Name { get; set; }


    [Display(Name = "Fiyat")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    [RegularExpression(@"^(([0-9.]?)*)+$", ErrorMessage = "Lütfen geçerli bir fiyat yazınız!")]
    [Range(minimum: 0, maximum: 20000000, ErrorMessage = "Fiyat {0} ile {1} arasında olmalıdır!")]
    public string Price { get; set; }


    [Display(Name = "Açıklama")]
    public string Description { get; set; }


    [Display(Name = "Görsel")]
    public IFormFile? Image { get; set; }

    [Display(Name = "İndirim Oranı")]
    [Required(ErrorMessage = "{0} alanı boş bırakılamaz!")]
    [RegularExpression(@"^[0-9]{1,2}$", ErrorMessage = "Lütfen geçerli bir oran yazınız!")]
    public string DiscountRate { get; set; }

    [Display(Name = "Katalog")]
    public IEnumerable<Guid> Catalogs { get; set; }

    [Display(Name = "Foto Galeri")]
    public IEnumerable<IFormFile>? Images { get; set; }
}
