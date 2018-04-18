<!DOCTYPE html>
<html>
<style>
<head>
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
body {font-family: Arial, Helvetica, sans-serif;}
* {box-sizing: border-box}

/* Full-width input fields */
input[type=text], input[type=password] {
    width: 100%;
    padding: 15px;
    margin: 5px 0 22px 0;
    display: inline-block;
    border: none;
    background: #f1f1f1;
}

input[type=text]:focus, input[type=password]:focus {
    background-color: #ddd;
    outline: none;
}

hr {
    border: 1px solid #f1f1f1;
    margin-bottom: 25px;
}

/* Set a style for all buttons */
button {
    background-color: #4CAF50;
    color: white;
    padding: 14px 20px;
    margin: 8px 0;
    border: none;
    cursor: pointer;
    width: 100%;
    opacity: 0.9;
}

button:hover {
    opacity:1;
}

/* Extra styles for the cancel button */
.cancelbtn {
    padding: 14px 20px;
    background-color: #f44336;
}

/* Float cancel and signup buttons and add an equal width */
.cancelbtn, .signupbtn {
  float: left;
  width: 50%;
}

/* Add padding to container elements */
.container {
    padding: 16px;
}

/* Clear floats */
.clearfix::after {
    content: "";
    clear: both;
    display: table;
}

/* Change styles for cancel button and signup button on extra small screens */
@media screen and (max-width: 300px) {
    .cancelbtn, .signupbtn {
       width: 100%;
    }
}
</style>
<body>


  <div class="container">
    <h1>Sign Up
	<div class="imgcontainer">
    <img src="http://localhost/job/c.jpg" alt="Avatar" class="avatar">
	
  </div>
  </h1>
    <p>Please fill up this form to create an account.</p>
    <hr>
    <form action="reg.php" method="post" enctype="multipart/form-data">
    Upload CV:
    <input type="file" name="fileToUpload" id="fileToUpload">
    <input type="submit" value="Upload CV" name="submit ">

     <br>
     <br>
	 <label for="\n name"><b>Name</b></label>
    <input type="text" placeholder="Enter Name" name="name" required>
	<label for="\n contact"><b>Contact</b></label>
	<br>
	<br>
    <input type="number_format" placeholder="Enter Number" name="contact" required>
	<br>
	<br>
	<br>
    <label for="\n email"><b>Email id</b></label>
    <input type="text" placeholder="Enter Email" name="email" required>

    <label for="psw"><b>Password</b></label>
    <input type="password" placeholder="Enter Password" name="psw" required>

    <label for="psw-repeat"><b>Retype Password</b></label>
    <input type="password" placeholder="Repeat Password" name="psw-repeat" required>
    
    <label>
      <input type="checkbox" checked="checked" name="remember" style="margin-bottom:15px"> Remember me
    </label>
    
    <p>By creating an account you agree to our <a href="#" style="color:blue">Terms & Privacy</a>.</p>
    <div class="clearfix">
      <button type="button" class="cancelbtn">Cancel</button>
      <button type="submit" class="signupbtn">Sign Up</button></a>
    </div>
  </div>
</form>

</body>
</html>