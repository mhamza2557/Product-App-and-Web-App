<!DOCTYPE html>
<html>
    <head>
        <title>
            Product Web App
        </title>
        <link rel="stylesheet" href="style.css">
        <style>
            #image {
                width: 15%;
            }
        </style>
    </head>
    <body>
        <?php
        $serverName = "Server Name Here";
        $connectionInfo = array("Database"=>"ProductApp_DB");
        $conn = sqlsrv_connect($serverName,$connectionInfo);
        $query = "SELECT * FROM ProductApp_Table";
        $stmt = sqlsrv_query($conn,$query);
        ?>
        <div class="wrapper">
            <header>
                <h1>Product Web App</h1>
                <h1 class="head">MH Hamza &copy;</h1>
            </header>

            <div class="content">
                <div class="inner">
                    <table border="1">
                        <tr>
                            <th>ID</th>
                            <th>Product Name</th>
                            <th>Purchase Time</th>
		                    <th>Price</th>
		                    <th>Customer Name</th>
		                    <th>Shop Name</th>
                            <th>Prduct Image</th>
                        </tr>
                        <?php
                        while ($row = sqlsrv_fetch_array($stmt,SQLSRV_FETCH_ASSOC)) {
                            echo "<tr>";
                            echo "<td>".$row['id']."</td>";
                            echo "<td>".$row['productname']."</td>";

                            $purchasedate = date_format($row['purchasedate'], 'd-m-Y H:i A');

                            echo "<td>".$purchasedate."</td>";
                            echo "<td>".$row['price']."</td>";
                            echo "<td>".$row['customername']."</td>";
                            echo "<td>".$row['shopname']."</td>";
                            echo "<td id='image'>".'<img onerror="this.src=\'No-image.png\'"  style="height:auto; width:100%"  src="data:image/jpeg;base64,'.base64_encode( $row['productimage'] ).'" />'."</td>";
                            echo "</tr>";
                        }
                        ?>
                    </table>
                </div>
            </div>
        </div>
    </body>
</html>
