//var selectedItemOld = localStorage.getItem("selectedItem");

//if (selectedItemOld) {
//    // Xóa selectedItem từ Local Storage
//    localStorage.removeItem("selectedItem");
//}



function DinhDangTienTe(element) {
    // Lấy giá trị từ nội dung của thẻ
    const giaTri = parseFloat(element.textContent); // Giả sử nội dung là số
    // Kiểm tra nếu giá trị hợp lệ (là số)
    if (!isNaN(giaTri)) {
        // Định dạng giá trị thành tiền tệ
        const dinhDangTienTe = giaTri.toLocaleString('vi-VN', { minimumFractionDigits: 0 })
        // Gán lại nội dung của thẻ với giá trị đã được định dạng
        element.textContent = dinhDangTienTe;
    }
}

/// 
//var listThuongHieu = document.querySelectorAll('.category-item'); //lấy tất cả elements từ thẻ li thương hiệu
//    listThuongHieu.forEach(item => {   // Lặp qua từng thẻ li
//        item.onclick = (e) => {
//                    // tìm xem đã có active hay chưa. nếu có thì remove
//                 let active = document.querySelector('.category-item.category-item--active');
//                 if (active) {
//                    active.classList.remove('category-item--active');
//                 }
//                 e.target.closest('.category-item').classList.add('category-item--active');

//    }
//})



const elements = document.querySelectorAll('.home-product-item__price-old');
const elementss = document.querySelectorAll('.home-product-item__price-current');
// Lặp qua từng phần tử và định dạng giá trị bên trong
elements.forEach((element) => {
    DinhDangTienTe(element);
});
elementss.forEach((element) => {
    DinhDangTienTe(element);
});


document.addEventListener("DOMContentLoaded", function () {
    // Lấy tất cả các phần tử <li> có class "category-item"
    var listItems = document.querySelectorAll(".category-item");

    // Xử lý sự kiện click cho mỗi phần tử <li>
    listItems.forEach(function (item) {
        item.addEventListener("click", function () {

            let active = document.querySelector('.category-item.category-item--active');
                 if (active) {
                   active.classList.remove('category-item--active');
                 }
           
            item.classList.add("category-item--active");

            // Lưu trạng thái vào Local Storage (để giữ lại sau khi tải lại trang)
            var itemId = item.getAttribute("data-id");
            sessionStorage.setItem("selectedItem1", itemId);
           // localStorage.setItem("selectedItem", itemId);
        });
    });

    // Kiểm tra nếu có trạng thái trước đó được lưu trong Local Storage và tái áp dụng nó
   // var selectedItem = localStorage.getItem("selectedItem");
    var selectedItem = sessionStorage.getItem("selectedItem2");

    if (selectedItem) {
        var selectedListItem = document.querySelector(".category-item[data-id='" + selectedItem + "']");
        if (selectedListItem) {
            selectedListItem.classList.add("category-item--active");
        }
    }


    
});

