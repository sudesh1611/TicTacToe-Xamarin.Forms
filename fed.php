<?php
$a = $_POST['name'];
$b = $_POST['secname'];
$c = $_POST['category'];
$d = $_POST['rating'];
echo $a; 
echo $b; 
echo $c;
echo $d; 
$conn = mysqli_connect("localhost", "root", "","user");
if (!$conn) {
    die("Connection failed: " . mysqli_connect_error());
}

$result = mysqli_query($conn,"insert into feedback (name,secname,category,rating) values ('$a','$b','$c','$d')") or die("opi");

   // header('Location: go.html');
/*	} 
	else{
    header('Location: index.html');
	}*/
//$_SESSION['login_user']= $a;
//echo $_SESSION['login_user'];
	//if($a == "admin" && $b== "admin123")
	//	header('Location: adminindex.php');
//	else if($a == $row[0] && $b==$row[1])
	//	header('Location: index.php');
//	else
	//	header('Location: signin.php');
//	}
mysqli_close($conn);

?>