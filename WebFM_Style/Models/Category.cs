using System;
using System.Collections.Generic;

namespace WebFM_Style.Models;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? SupplierId { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public DateTime? Cdt { get; set; }
    /// <summary>
    /// //nè
    /// Khoan, còn trong cái detail dí delete y v lun nefddr đr e 
    /// hoặc em copy quăng cho con bot 
    /// Nó làm được mà 
    /// Tại e k pt quăng code dô chỗ nào ấái cái nào CDT 
    /// Hay thuộc tính em muốn đổi 
    /// Hoặc copy hết 
    /// R em nói ý em muốn sửa 
    /// Là nó xủa 
    /// Rồi đen về chạy 
    /// Xem đúng ko 
    /// Ko đúng kêu nó sửa 
    /// Theo ý 
    /// Okee a out nhê
    /// Khoan
    /// Còn mấy cái trạng thái e tự sửa luôn hả
    /// đr 
    /// có mấy trang có sẵn rồi đó 
    /// Em copy bỏ qua 
    /// Là được 
    /// Ko em kêu bót nó sửa cx được mà 
    /// Okee a out nhé 
    /// đơn giản như đang giỡ 
    /// Mà
    /// Có cần chạy web dí app chung 1 lượt k a
    /// a viết đọc lập 
    /// Nên ko cần 
    /// để a 2 cái chung 1 cchoox 
    /// đẻ nó lấy được ảnh của nhau là đc 
    /// Hả
    /// như z đó 
    /// Là oke 
    /// Ủa anh mới làm j v
    /// A cho em xem thư mục chứ làm gì 
    /// Là để 2 cái chung code đó chung thư mục là đc đk
    /// đúng 
    /// Okee
    /// Còn đóng gói nó sao a a hướng dẫn trong docs giờ a làm lại 1 lần cho xem 
    /// Lấy đt 
    /// Quay lại 
    /// 
    /// </summary>
    public virtual ICollection<ProductType> ProductTypes { get; set; } = new List<ProductType>();

    public virtual Supplier? Supplier { get; set; }
}
