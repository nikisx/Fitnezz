function Validate() {
    let mealPLanName = document.getElementById("mealPlanNameInput").value;
    let imgSrc = document.getElementById("imgInput").value;
    if (mealPLanName.length < 5 || mealPLanName.length > 30 || imgSrc === "") {
        alert("Meal plan name should be between  5 and 30 characters and the url should be valid");
        return;
    }
}