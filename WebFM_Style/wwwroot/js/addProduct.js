
$(document).ready(function () {
    $('.add-to-cart').on('click', function (e) {
        e.preventDefault();
        var productId = $(this).data('id'); // Get product ID from data attribute
        $.ajax({
            url: '@Url.Action("GetProductDetails", "Products")', // Adjust the action and controller as needed
            type: 'GET',
            data: { id: productId },
            success: function (data) {
                // Assuming 'data' contains the product details
                $('#modalProductImage').attr('src', '/contents/Images/Product/' + data.images[0]); // Set product image (first image)
                $('#modalProductName').text(data.name); // Set product name
                $('#modalProductPrice').html('$' + data.price.toFixed(2)); // Set product price
                $('#modalProductDescription').text(data.description); // Set product description
                $('#modalProductName').text(data.name); // Set product description
                $('#productId').val(data.id); // Set product description


                // Set unique sizes
                var sizesHtml = '';
                var uniqueSizes = new Set(); // Create a Set to track unique sizes
                data.sizeColors.forEach(function (sizeColor) {
                    if (!uniqueSizes.has(sizeColor.size.size1)) {
                        uniqueSizes.add(sizeColor.size.size1); // Add size to the Set
                        sizesHtml += '<label><input type="radio" name="size" value="' + sizeColor.size.size1 + '">' + sizeColor.size.size1 + '</label>';
                    }
                });
                $('#modalProductSizes').html(sizesHtml);

                // Set unique colors
                var colorsHtml = '';
                var uniqueColors = new Set(); // Create a Set to track unique colors
                data.sizeColors.forEach(function (sizeColor) {
                    if (!uniqueColors.has(sizeColor.color.color1)) {
                        uniqueColors.add(sizeColor.color.color1); // Add color to the Set
                        colorsHtml += '<label><input type="radio" name="size" value="' + sizeColor.color.color1 + '">' + sizeColor.color.color1 + '</label>';

                    }
                });
                $('#modalProductColors').html(colorsHtml);


                // Populate thumbnails
                var thumbnailsHtml = '';
                var tabContentHtml = '';
                data.images.forEach(function (image, index) {
                    thumbnailsHtml += `
                                <li class="nav-item">
                                    <a class="nav-link ${index === 0 ? 'active' : ''}" data-toggle="tab" href="#tabs-${index}" role="tab">
                                                <div class="product__details__pic__item">
                                            <img src="/contents/Images/Product/${image}" alt=""> <!-- Image will be set here -->
                                        </div>
                                    </a>
                                </li>
                            `;
                    tabContentHtml += `
                                <div class="tab-pane ${index === 0 ? 'active' : ''}" id="tabs-${index}" role="tabpanel">
                                    <div class="product__details__pic__item">
                                        <img src="/contents/Images/Product/${image}" alt=""> <!-- Image will be set here -->
                                    </div>
                                </div>
                            `;
                });
                $('.nav-tabs').html(thumbnailsHtml); // Populate the thumbnails
                $('.tab-content').html(tabContentHtml); // Populate the tab content

                $('#productDetailsModal').modal('show'); // Show the modal
            },
            error: function () {
                alert('Error loading product details.');
            }
        });
    });
});
// Khi người dùng chọn size
$(document).on('click', '#modalProductSizes label', function () {
    $('#modalProductSizes label').removeClass('active');
    $(this).addClass('active');
    $('#selectedSize').val($(this).find('input').val()); // Ghi size được chọn vào input ẩn
});

// Khi người dùng chọn color
$(document).on('click', '#modalProductColors label', function () {
    $('#modalProductColors label').removeClass('active');
    $(this).addClass('active');
    $('#selectedColor').val($(this).find('input').val()); // Ghi color được chọn vào input ẩn
});



$(document).on('click', '.primary-btn', function (e) {
    e.preventDefault();

    var selectedSize = $('#selectedSize').val();
    var selectedColor = $('#selectedColor').val();
    var productId = $('#productId').val();

    // Kiểm tra xem người dùng đã chọn kích cỡ và màu sắc chưa
    if (!selectedSize || !selectedColor) {
        new Noty({
            text: 'Vui lòng chọn kích cỡ và màu sắc trước khi thêm vào giỏ hàng.',
            type: 'error',
            theme: 'mint',
            timeout: 3000,
            progressBar: true
        }).show();
        return;
    }


    $.ajax({
        url: '@Url.Action("AddToCart", "Orders")',
        type: 'POST',
        data: {
            productId: productId,
            size: selectedSize,
            color: selectedColor
        },
        success: function (response) {
            if (response.success) {
                new Noty({
                    text: response.message || 'Thêm sản phẩm vào giỏ hàng thành công!',
                    type: 'success',
                    theme: 'mint',
                    timeout: 3000,
                    progressBar: true
                }).show();
                $('#selectedSize').val(null);
                $('#selectedColor').val(null);

                $('#productDetailsModal').modal('hide');
            } else {
                new Noty({
                    text: response.message || 'Không thể thêm sản phẩm vào giỏ hàng.',
                    type: 'warning',
                    theme: 'mint',
                    timeout: 3000,
                    progressBar: true
                }).show();
            }
        },
        error: function () {
            new Noty({
                text: 'Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng.',
                type: 'error',
                theme: 'mint',
                timeout: 3000,
                progressBar: true
            }).show();
        }
    });
});

