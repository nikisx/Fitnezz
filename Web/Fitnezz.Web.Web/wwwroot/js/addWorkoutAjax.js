
    $(document).ready(function () {
        //function will be called on button click having id btnsave
        $("#btnSave").click(function () {
            let workoutName = document.getElementById("workoutName").value;

            if (workoutName.length < 5 || workoutName.length > 30) {
                alert("Workout name should be between  5 and 30 characters");
                return;
            }
            $.ajax(
                {
                    type: "GET", //HTTP POST Method  
                    url: "/Workouts/Create", // Controller/View   
                    data: { //Passing data  
                        workoutName: $("#workoutName").val(), //Reading text box values using Jquery   
                        isPublic: $("#isPublic").val(),
                    },
                    dataType: "text",
                    contentType: "application/json; charset=utf-8",
                    success: function () {
                        location.reload();
                    },
                    failure: function(){
                        alert("Workout name should be between  5 and 30 characters");
                }
                });

        });
    });
