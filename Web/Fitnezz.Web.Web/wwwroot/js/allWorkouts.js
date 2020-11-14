let btn = document.getElementById("workoutBtn");
let form = document.getElementById("createWorkout");
let cancelBtn = document.getElementById("canceltBtn");

btn.addEventListener(("click"), () => {
    form.style.display = "block";

});
cancelBtn.addEventListener(("click"), () => {
    form.style.display = "none";
});

function Alert() {
    let workoutName = document.getElementById("workoutName").value;
    if (workoutName.length < 5 || workoutName.length > 30) {
        alert("Workout name should be between  5 and 30 characters");
        return;
    }

}