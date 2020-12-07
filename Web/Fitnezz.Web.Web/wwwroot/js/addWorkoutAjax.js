
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
                        var newRowContent = "<tr class = \"row\"><td style=\"font-size: 20px; color: aliceblue\" class=\"col-md - 6 \">"+ workoutName + "</td>" +
                            "<td style=\"font-size: 20px; color: aliceblue\" class=\"col-md - 6 \">0</td>" +
                            "<td class=\"col-lg - 6\"><div class=\"button-holder\"><a href=\"/Workouts/All\" class=\"btn btn-warning\">Details</a></div></td></tr>" +
                            "<td style=\"position: absolute; top:37%; left: 5%\">\r\n<input type=\"text\" name=\"username\" placeholder=\"Username\" style=\"width: 100px\" />\r\n<button type=\"submit\" class=\"btn btn-sm\"> Add to User</button></td>" +
                            "<td style=\"position: absolute;right: 12%; top:36.6%;\"><button type=\"submit\" class=\"btn delete\" data-id=\"null\" data-controller=\"Workouts\" data-action=\"Delete\"data-body-message=\"Delete this workout?\">Delete</button></td>";

                        $(newRowContent).prependTo($("#tblEntAttributes"));
                    },
                    failure: function(){
                        alert("Workout name should be between  5 and 30 characters");
                }
                });

        });
    });
