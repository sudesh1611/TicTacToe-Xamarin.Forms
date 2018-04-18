<!DOCTYPE html>
<html>
<head>
<meta name="viewport" content="width=device-width, initial-scale=1">
<style>
body {font-family: Arial, Helvetica, sans-serif;}
form {border: 3px solid #f1f1f1;}

input[type=text], input[type=password] {
    width: 100%;
    padding: 12px 20px;
    margin: 8px 0;
    display: inline-block;
    border: 1px solid #ccc;
    box-sizing: border-box;
}

button {
    background-color: #4CAF50;
    color: white;
    padding: 14px 20px;
    margin: 8px 0;
    border: none;
    cursor: pointer;
    width: 100%;
}

button:hover {
    opacity: 0.8;
}

.cancelbtn {
    width: auto;
    padding: 10px 18px;
    background-color: #f44336;
}

.imgcontainer {
    text-align: center;
    margin: 24px 0 12px 0;
}

img.avatar {
    width: 40%;
    border-radius: 50%;
}

.container {
    padding: 16px;
}

span.psw {
    float: right;
    padding-top: 16px;
}

/* Change styles for span and cancel button on extra small screens */
@media screen and (max-width: 300px) {
    span.psw {
       display: block;
       float: none;
    }
    .cancelbtn {
       width: 100%;
    }
}
</style>
<script>
function validateForm() 
{var y = document.forms["form"]["name"].value;	v
ar letters = /^[A-Za-z]+$/;
if (y == null || y == "") {alert("Name must be filled out.");
return false;}var z =document.forms["form"]["college"].value;if (z == null || z == "") 
{alert("college must be filled out.");
return false;}var x = document.forms["form"]["email"].value;var atpos = x.indexOf("@");
var dotpos = x.lastIndexOf(".");
if (atpos<1 || dotpos<atpos+2 || dotpos+2>=x.length) {alert("Not a valid e-mail address.");
return false;}var a = document.forms["form"]["password"].value;if(a == null || a == ""){alert("Password must be filled out");
return false;}if(a.length<5 || a.length>25){alert("Passwords must be 5 to 25 characters long.");
return false;}
var b = document.forms["form"]["cpassword"].value;if (a!=b){alert("Passwords must match.");
return false;}}
</script>
</head>
<body>

<h2>Employee Login</h2>
<form action="reg.php" method="post">
  <div class="imgcontainer">
   <!-- <img src="http://localhost/job/a.jpg" alt="Avatar" class="avatar">-->
	
  </div>
  
<form action="upload.php" method="post" enctype="multipart/form-data">
    Upload photo:
    <input type="file" name="fileToUpload" id="fileToUpload">
    <input type="submit" value="Upload Image" name="submit">
</form>
  <div class="container">
    <label for="uname"><b>Username</b></label>
    <input type="text" placeholder="Enter Username" name="name" required>

    <label for="psw"><b>Password</b></label>
    <input type="password" placeholder="Enter Password" name="psw" required>
        
    <button type="submit">Login</button>
    <label>
      <input type="checkbox" checked="checked" name="remember"> Remember me
    </label>
  </div>

  <div class="container" style="background-color:#f1f1f1">
    <button type="button" class="cancelbtn">Cancel</button>
    <span class="psw">Forgot <a href="#">password?</a></span>
  </div>
  
  
</form>

</body>
</html>