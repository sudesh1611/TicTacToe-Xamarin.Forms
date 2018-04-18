<!DOCTYPE html>
<html>
<body>

<h2>Feedback Form</h2>
<form action="fed.php" method="post">

  First name:<br>
  <input type="text" name="name" value="">
  <br>
  Last name:<br>
  <input type="text" name="secname" value="">
  <br><br>
  Who should we say is contacting us? <br>
  <input type="radio" name="category" value="company"> Company<br>
  <input type="radio" name="category" value="jobseeker"> Job Seeker<br>
  
  <br>
  <br>
  Rate your experience: <br>
  <input type="radio" name="rating" value="excellent"> Excellent<br>
  <input type="radio" name="rating" value="average"> Average<br>
  <input type="radio" name="rating" value="poor"> Poor<br>
  <input type="submit" value="Submit">
</form> 


</body>
</html>
