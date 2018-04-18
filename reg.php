<?php
$a = $_POST['name'];
$b = $_POST['contact'];
$c = $_POST['email'];
$d = $_POST['psw'];
$f = $_POST['psw-repeat']
$conn = mysqli_connect("localhost", "root", "","user");
if (!$conn) {
    die("Connection failed: " . mysqli_connect_error());
}

$result = mysqli_query($conn,"insert into login (name,contact,email,password, rpassword) values ('$a','$b','$c','$d', '$f')");
/*
if($result){
	session_start();
	$_SESSION['login_user']= $e;
    header('Location: index.html');
	} 
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