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
            $('#idthuonghieu').val('');

            $('#modal-title').text('Thêm thương hiệu mới');
            homeController.resetForm();
            $('#btnSubmit').show();
            $('#thuongHieuModal').modal('show');
            $(".resettable").prop('readonly', false);
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
                        $('#modal-title').text('Chi tiết thương hiệu');
                        $('#btnSubmit').hide();
                        $('#MaTH').val(data.th.MaTH);
                        $('#TenThuongHieu').val(data.th.TenThuongHieu);
                        $('#CodeThuongHieu').val(data.th.CodeThuongHieu);
                        $(".resettable").prop('readonly', true);
                        $('#thuongHieuModal').modal('show');

                    } else {
                        alert(data.msg);
                    }
                }
            })

        });


        // Cập nhật thương hiệu
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
                        $('#idthuonghieu').val(data.th.MaTH);
                        $('#modal-title').text('Cập nhật');
                        $('#btnSubmit').show();
                        $('#TenThuongHieu').val(data.th.TenThuongHieu);
                        $('#CodeThuongHieu').val(data.th.CodeThuongHieu);
                        $(".resettable").prop('readonly', false);
                        $('#thuongHieuModal').modal('show');
                    } else {
                        alert(data.msg);
                    }
                }
            })

        });

        $('#btnCloseModal').click(function () {
            $("#thuongHieuModal").modal("hide");

        });

        
        // Xác nhận Thêm hoặc Sửa sản phẩm
        $('#btnSubmit').off('click').on('click', function () {
            let idThuongHieu = $('#idthuonghieu');
            let formData = new FormData($("#addForm")[0]);

            if (idThuongHieu.val().trim().length == 0) {

                // Thêm mới
                $.ajax({
                    url: 'ThemThuongHieu', // Controller 
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
                            $("#thuongHieuModal").modal("hide");
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
                            $("#thuongHieuModal").modal("hide");

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
                url: 'XoaThuongHieu',
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
            url: '/admin/ThuongHieu/DsThuongHieu',
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
                            MaTH: item.maTH,
                            TenThuongHieu: item.tenThuongHieu,
                            CodeThuongHieu: item.codeThuongHieu,
                            //Status: item.Status == true ? "<span class=\"label label-success\">Actived</span>" : "<span class=\"label label-danger\">Locked</span>"
                        });

                    });

                    $('#tblData').html(html);
                    homeController.paging(response.total, function () {
                        homeController.loadData();
                    }, changePageSize);
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
            first: "<<",
            next: ">",
            last: ">>",
            prev: "<",
            visiblePages: 5,
            onPageClick: function (event, page) {
                homeconfig.pageIndex = page;
                setTimeout(callback, 150);
            }
        });
    },

    resetForm: function () {
        $(".resettable").val("");
    },

};

$(document).ready(function () {
    homeController.init();

});
