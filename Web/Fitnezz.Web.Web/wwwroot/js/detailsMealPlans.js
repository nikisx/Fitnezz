function Validate() {
    let mealName = document.getElementById("mealName").value;
    if (mealName.length < 5 || mealName.length > 40) {
        alert("Meal name should be between  5 and 30 characters and the url should be valid");
        return;
    }
}
