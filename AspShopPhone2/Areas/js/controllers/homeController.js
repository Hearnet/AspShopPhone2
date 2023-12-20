var homeconfig = {
    pageSize: 5,
    pageIndex: 1,
};


var homeController = {
    init: function () {
        homeController.loadData();
        homeController.registerEvent();
    },
    registerEvent: function () {

        // click vào nút Thêm mới
        $('#btnADD').click(function () {
            $('#iddienthoai').val('');

            $('#modal-title').text('Thêm mới sản phẩm');
            homeController.resetForm();
            $('#imgProduct').hide();
            $('#fileInput').show();
            $('#btnSubmit').show();
            $('#sanPhamModal').modal('show');
            $(".resettable").prop('readonly', false);
            $("#MaTH").prop("disabled", false);
        });

        // Xoá sản phẩm
        $("body").on("click", "#btnXoa", function () {
            // Lấy giá trị ID của thẻ tr cha của nút đã được click
            var id = $(this).data('id');
            

            bootbox.confirm("Bạn chắc chắn muốn xoá?", function (result) {
                if (result) {
                    homeController.deleteData(id);
                }
            });

        });

        $("body").on("keypress", "#txtNameS", function (event) {
            if (event.which === 13) {
                homeController.loadData(true);
            }
        });
        //$('#btnSearch').off('click').on('click', function () {
        //    homeController.loadData(true);
        //});
        // Lấy chi tiết Sản Phẩm
        // do btnChiTiet đã được gán sự kiện trước khi nút html được tạo ra nên sau 
        // khi gọi ajax để render ra btn sẽ không nhận dc sự kiện nữa
        // ta phải dùng sự kiện click gán cho body ó sẽ kiểm tra phần tử đã click 
        // và các phần tử con của nó, kể cả những phần tử được tạo động sau khi trang được tải.
        // Điều này cho phép bạn xử lý sự kiện cho các nút được tạo động từ AJAX.
        $("body").on("click", ".btnChiTiet", function () {
            // Lấy giá trị ID của thẻ tr cha của nút đã được click
            var id = $(this).data('id');

            $.ajax({
                url: 'ChiTiet',
                type: 'GET',
                dataType: 'json',
                data: {
                    id: id
                },
                success: function (data) {
                    if (data.code == 200) {
                        $('#modal-title').text('Chi tiết sản phẩm');
                        $('#fileInput').hide();
                        $('#btnSubmit').hide();
                        $('#imgProduct').show();

                        $('#TenDienThoai').val(data.dt.TenDienThoai);
                        $('#MaTH').val(data.dt.MaTH);
                        $('#GiaBan').val(data.dt.GiaBan);
                        $('#GiaGiam').val(data.dt.GiamGia);
                        $('#MoTa').val(data.dt.MoTa);
                        $('#SoLuongTon').val(data.dt.SoLuongTon);
                        $('#imgProduct').attr("src", "/assets/img/sanPham/" + data.dt.AnhBia);

                        $(".resettable").prop('readonly', true);
                        $("#MaTH").prop("disabled", true);
                        $('#sanPhamModal').modal('show');

                    } else {
                        alert(data.msg);
                    }
                }
            })

        });


        // Cập nhật sản phẩm
        $("body").on("click", ".btnUpDate", function () {
            // Lấy giá trị ID của thẻ tr cha của nút đã được click
            var id = $(this).data('id');

            $.ajax({
                url: 'ChiTiet',
                type: 'GET',
                dataType: 'json',
                data: {
                    id: id
                },
                success: function (data) {
                    if (data.code == 200) {
                        $('#iddienthoai').val(data.dt.MaDT);

                        $('#modal-title').text('Cập nhật sản phẩm');
                        $('#fileInput').show();
                        $('#btnSubmit').show();
                        $('#imgProduct').show();

                        $('#TenDienThoai').val(data.dt.TenDienThoai);
                        $('#MaTH').val(data.dt.MaTH);
                        $('#GiaBan').val(data.dt.GiaBan);
                        $('#GiaGiam').val(data.dt.GiamGia);
                        $('#MoTa').val(data.dt.MoTa);
                        $('#SoLuongTon').val(data.dt.SoLuongTon);
                        $('#imgProduct').attr("src", "/assets/img/sanPham/" + data.dt.AnhBia);
                        $(".resettable").prop('readonly', false);
                        $("#MaTH").prop("disabled", false);
                        $('#sanPhamModal').modal('show');

                    } else {
                        alert(data.msg);
                    }
                }
            })

        });

        $('#btnCloseModal').click(function () {
            $("#sanPhamModal").modal("hide");

        });

        
        // Xác nhận Thêm hoặc Sửa sản phẩm
        $('#btnSubmit').off('click').on('click', function () {
            let idDienThoai = $('#iddienthoai');
            let formData = new FormData($("#addProductForm")[0]);

            if (idDienThoai.val().trim().length == 0) {

                // Thêm mới
                $.ajax({
                    url: 'ThemSanPham', // Thay YourController bằng tên của controller của bạn
                    type: 'POST',
                    dataType: 'json',
                    data: formData,
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        // Xử lý kết quả trả về từ action ở đây
                        if (response.code == 200) {
                            bootbox.alert(response.msg)
                            homeController.loadData(true);
                            homeController.resetForm();
                            $("#sanPhamModal").modal("hide");
                        } else {
                            alert(response.msg)
                        }
                    }

                });

            } else {
                // Cập nhật
                $.ajax({
                    url: 'CapNhat', // Thay YourController bằng tên của controller của bạn
                    type: 'POST',
                    dataType: 'json',
                    data: formData,
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        // Xử lý kết quả trả về từ action ở đây
                        if (response.code == 200) {
                            bootbox.alert(response.msg)
                            homeController.loadData();
                            homeController.resetForm();
                            $("#sanPhamModal").modal("hide");

                        } else {
                            alert(response.msg)
                        }
                    }

                });
            }

        })


    },

    deleteData: function (id) {
        $.ajax({
                url: 'XoaSanPham',
                type: 'POST',
                dataType: 'json',
                data: {
                    id: id
                },
                success: function (data) {
                    if (data.code == 200) {
                        bootbox.alert(data.msg);
                        homeController.loadData(true);
                    } else {
                        bootbox.alert(data.msg);
                    }
                }
            })
    },

    loadData: function (changePageSize) {
        var name = $('#txtNameS').val().trim();
        $.ajax({
            url: '/admin/SanPham/DsDienThoai',
            type: 'GET',
            data: {
                name: name,
                page: homeconfig.pageIndex,
                pageSize: homeconfig.pageSize
            },
            success: function (response) {
                if (response.code == 200) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();

                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.id,
                            MaTH: item.idTH,
                            TenDienThoai: item.tenDienThoai,
                            GiaBan: item.giaBan,
                            MoTa: item.moTa,
                            AnhBia: item.anhBia
                            //Status: item.Status == true ? "<span class=\"label label-success\">Actived</span>" : "<span class=\"label label-danger\">Locked</span>"
                        });

                    });

                    $('#tblData').html(html);
                    homeController.paging(response.total, function () {
                        homeController.loadData();
                    }, changePageSize);
                        const elements = document.querySelectorAll('.home-product-item__price');
                        elements.forEach((element) => {
                            homeController.DinhDangTienTe(element);
                        })



                }
            }
        });
    },

    // Phân trang
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / homeconfig.pageSize);

        //Unbind pagination if it existed or click change pagesize
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: "Đầu",
            next: "Tiếp",
            last: "Cuối",
            prev: "Trước",
            visiblePages: 5,
            onPageClick: function (event, page) {
                homeconfig.pageIndex = page;
                setTimeout(callback, 150);
            }
        });
    },

    resetForm: function () {
        $(".resettable").val("");
        $(".resettable[type='file']").replaceWith(function () {
            return $(this).val('');
        });
    },

    DinhDangTienTe: function (element) {
        // Lấy giá trị từ nội dung của thẻ
        const giaTri = parseFloat(element.textContent); // Giả sử nội dung là số
        // Kiểm tra nếu giá trị hợp lệ (là số)
        if (!isNaN(giaTri)) {
            // Định dạng giá trị thành tiền tệ
            const dinhDangTienTe = giaTri.toLocaleString('vi-VN', { minimumFractionDigits: 0 })
            // Gán lại nội dung của thẻ với giá trị đã được định dạng
            element.textContent = dinhDangTienTe + " VNĐ";
        }
    }

};

$(document).ready(function () {
    homeController.init();




});
