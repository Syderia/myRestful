<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookTestPage.aspx.cs" Inherits="myRestful.webPage.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" href="Scripts/jquery-ui-1.12.1/jquery-ui.css" />
    <script src="Scripts/jquery-ui-1.12.1/external/jquery/jquery.js"></script>
    <script src="Scripts/jquery-ui-1.12.1/jquery-ui.js"></script>
    <script type="text/javascript">

        function getBooks() {
            $.ajax({
                type: "Get",
                url: "api/books",
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    var bookList = JSON.parse(data);
                    $('#books').empty(); // Clear the table body.
                    var row = "<tr><th>書號</th><th>書名</th><th>價格</th><th>進貨日</th><th>銷售量</th><th>排名</th></tr>";
                    $.each(bookList, function (key, val) {
                        row += '<tr><td>' + val.BookId + '</td><td>' + val.BookName + '</td><td>' + val.Price + '</td><td>' + val.ImportDate + '</td><td>' + val.SaleNumber + '</td><td>' + val.Rank + '</td></tr>';
                    });
                    $('#books').append(row);
                },
                error: function (data) {
                    var dataStr = $(data).find("string");
                    alert(dataStr);
                }
            });
        }




        $(function () {


            //==============jquery ui================
            $("#importDate").datepicker();
            
           //==============jquery ui=================

           //==============initialize=================
            getBooks();


           //==============initialize=================

           //==============the button event methods================
           
            $("#btnSearch").click(function () {
                if (CheckField("search")) {
                    var id = $("#searchId").val();
                    $.getJSON("api/books/" + id,
                        function (data) {
                            var book = JSON.parse(data);
                            $('#books').empty(); // Clear the table body.
                            var row = "<tr><th>書號</th><th>書名</th><th>價格</th><th>進貨日</th><th>銷售量</th><th>排名</th></tr>";
                          
                            row = row + '<tr><td>' + book.BookId + '</td><td>' + book.BookName + '</td><td>' + book.Price + '</td><td>' + book.ImportDate + '</td><td>' + book.SaleNumber + '</td><td>' + book.Rank + '</td></tr>';
                           
                            $('#books').append(row);
                        });
                }
            });

            $("#btnCreate").click(function () {
                if (CheckField("create")) {
                    var bookId = $("#id").val();
                    var bookName = $("#name").val();
                    var price = $("#price").val();
                    var importDate = $("#importDate").val();
                    var saleNumber = $("#saleNumber").val();
                    var rank = $("#rank").val();

                    var book = { 'bookId': bookId, 'bookName': bookName, 'price': price, 'importDate': importDate, 'saleNumber': saleNumber, 'rank': rank };
                    $.ajax({
                        type: "POST",
                        url: "api/books",
                        dataType: 'Json',
                        data: JSON.stringify(book),
                        contentType: 'application/Json',
                        success: function (data) {
                            alert("post success");

                        },
                        error: function (data) {
                            var dataStr = $(data).find("string");
                            alert(dataStr);

                        }
                    });
                }
            });

            $("#btnUpdate").click(function () {
                if (CheckField("update")) {
                    var bookId = $("#id").val();
                    var bookName = $("#name").val();
                    var price = $("#price").val();
                    var importDate = $("#importDate").val();
                    var saleNumber = $("#saleNumber").val();
                    var rank = $("#rank").val();

                    var book = { 'bookId': bookId, 'bookName': bookName, 'price': price, 'importDate': importDate, 'saleNumber': saleNumber, 'rank': rank };
                    $.ajax({
                        type: "PUT",
                        url: "api/books/" + bookId,
                        dataType: 'Json',
                        data: JSON.stringify(book),
                        contentType: 'application/Json',
                        success: function (data) {
                            alert("UPDATE success");

                        },
                        error: function (data) {
                            var dataStr = $(data).find("string");
                            alert(dataStr);

                        }
                    });
                }
            });

            $("#btnDelete").click(function () {
                if (CheckField("delete")) {
                    var bookId = $("#id").val();
                    $.ajax({
                        type: "Delete",
                        url: "api/books/" + bookId,
                        success: function (data) {
                            alert("Delete success");

                        },
                        error: function (data) {
                            var dataStr = $(data).find("string");
                            alert(dataStr);

                        }
                    });
                }
            });
            //==============the button event methods================
            
            //==============the other functions================
            //check field
            function CheckField(type) {
                var flag = false;

                var bookId = $("#id").val();
                var bookName = $("#name").val();
                var price = $("#price").val();
                var importDate = $("#importDate").val();
                var saleNumber = $("#saleNumber").val();
                var rank = $("#rank").val();
                var searchId = $("#searchId").val();
                var msg = "";

                switch (type) {
                    case "create":
                        if (bookId == "" || bookName == "" || price == "" || importDate == "" || saleNumber == "" || saleNumber == "") {
                            msg = "欄位未正確填寫";
                            flag = false;
                        } else {
                            flag = true;
                        }
                        break;
                    case "update":
                        if (bookId == "") {
                            msg = "修改時id為必填欄位";
                            flag = false;
                        } else {
                            flag = true;
                        }
                        break;
                    case "search":
                        if (searchId == "") {
                            msg = "查詢時id為必填欄位";
                            flag = false;
                        } else {
                            flag = true;
                        }
                        break;
                    case "delete":
                        if (bookId == "") {
                            msg = "刪除時id為必填欄位";
                            flag = false;
                        } else {
                            flag = true;
                        }
                        break;

                }
                if (msg != "") {

                    alert(msg);
                }
                return flag;
            }


            //==============the other functions================


        });


    </script>
</head>
<body>
    <div>
        <div>
            <label>書號:</label>
            <input type="text" id="id" />
        </div>
        <div>
            <label>書名:</label>
            <input type="text" id="name" />
        </div>
        <div>
            <label>價格:</label>
            <input type="text" id="price" />
        </div>
        <div>
            <label>進貨日:</label>
            <input type="text" id="importDate" />
        </div>
        <div>
            <label>銷售量:</label>
            <input type="text" id="saleNumber" />
        </div>
        <div>
            <label>排名:</label>
            <input type="text" id="rank" />
        </div>
        <div>
            <input type="submit" id="btnCreate" value="新增一筆" />
        </div>
    </div>
    <div>
        <input type="text" id="searchId" />
        <input type="button" id="btnSearch" value="搜尋" />
    </div>
    <div>
        <input type="submit" id="btnUpdate" value="更新資料" />
        <input type="button" id="btnDelete" value="刪除資料" />
    </div>

    <div>
        <table id="books" border="1">
            <tr>
                <th>書號</th>
                <th>書名</th>
                <th>價格</th>
                <th>進貨日</th>
                <th>銷售量</th>
                <th>排名</th>
            </tr>
        </table>
    </div>
</body>
</html>
