

function slideMenu(objectNoFixed,objectMenu,menu_items, spacingItems) {
    cssObjects(objectNoFixed,objectMenu,menu_items);
    placeItems(objectNoFixed,objectMenu,menu_items, spacingItems);

    var currentPosition; // Instant movement position
    var startPosition; // Location when first clicked with mouse or touch
    var beforePosition = 0; // The most recent motion position.
    //The width of the object containing the menu is fixed, it does not change. The width of the div containing this object shrinks according to the screen size. 
    //In this case, the overflowing amount of the fixed object is recorded. The object will be able to move this amount.
    var maxOverflow = objectMenu.clientWidth - objectNoFixed.clientWidth; 

    objectMenu.onmousedown = function(e){
        startPosition = e.pageX;
        objectMenu.addEventListener("mousemove", onMouseMove);
        objectMenu.onmouseleave = function(e){
            objectMenu.removeEventListener("mousemove", onMouseMove);
            beforePosition = currentPosition;
        }
    }

    objectMenu.onmouseup = function(e){
        objectMenu.removeEventListener("mousemove", onMouseMove);
        beforePosition = currentPosition;
    }

    objectMenu.ontouchstart = function(e){
        startPosition = e.touches[0].pageX;
        objectMenu.addEventListener("touchmove", onTouchMove);
    }

    objectMenu.ontouchend = function(e){
        objectMenu.removeEventListener("touchmove", onTouchMove);
        beforePosition = currentPosition;
    }

    function onTouchMove(e){
        var objectMenuLeft = objectMenu.style.left;
        objectMenuLeft = objectMenuLeft.replace("px","");

        //The difference between the starting position and the moving position is taken. 
        //This value is negative in left-hand movements. In the right movements, it is positive.
        if(e.touches[0].pageX - startPosition < 0){ //Moving Left
            //If the amount of overflow on the left side has not exceeded the maximum overflow amount, the object is shifted to the left side.
            if(objectMenuLeft >= -(maxOverflow)){
                inMotionTouch(e); //In motion
            }
        }else { // Moving Right
            //In movements to the right, the object's closeness to the left should not exceed the zero position.
            if(objectMenuLeft < 0){
                inMotionTouch(e); //In motion
            }
        }
    }

    function inMotionTouch(e){
        currentPosition = e.touches[0].pageX - startPosition + beforePosition;
        objectMenu.style.left = currentPosition + "px";
    }

    function placeItems (objectNoFixed, objectMenu, menu_items, spacingItems ) {
        var count_item = menu_items.length;
        var left_position = 0, before_sum = 0, blank_width = 0, _blank_width = spacingItems, before_item_width = 0;

        for(var i=0; i<count_item; i++){
            left_position = before_sum + before_item_width + blank_width;
            menu_items[i].style.left = left_position + "px";
            before_sum = left_position;
            before_item_width = menu_items[i].clientWidth;
            if(i == 0) { blank_width = _blank_width }
        }
        objectNoFixed.style.maxWidth = before_sum + before_item_width + "px";
        objectMenu.style.width = before_sum + before_item_width + "px";
    }

    function onMouseMove(e){
        var objectMenuLeft = objectMenu.style.left;
            objectMenuLeft = objectMenuLeft.replace("px","");

        //The difference between the starting position and the moving position is taken. 
        //This value is negative in left-hand movements. In the right movements, it is positive.
        if(e.pageX - startPosition < 0){ //Moving Left
            //If the amount of overflow on the left side has not exceeded the maximum overflow amount, the object is shifted to the left side.
            if(objectMenuLeft >= -(maxOverflow)){
                inMotion(e); //In motion
            }
        }else { // Moving Right
            //In movements to the right, the object's closeness to the left should not exceed the zero position.
            if(objectMenuLeft < 0){
                inMotion(e); //In motion
            }
        }
    }

    function inMotion(e){
        currentPosition = e.pageX - startPosition + beforePosition;
        objectMenu.style.left = currentPosition + "px";
    }
    
    function cssObjects(objectNoFixed,objectMenu,menu_items) {
        objectNoFixed.setAttribute(
            "style",
            "position:relative; overflow:hidden; margin:auto; height:40px;"
        );
        objectMenu.setAttribute(
            "style",
            "position:absolute; top:0px; left:0px; height:40px;"
        );
        menu_items.forEach(item => {
            item.setAttribute(
                "style",
                "position:absolute; left:0; top:5px;"
            );

            item.childNodes.forEach(node => {
                node.setAttribute(
                    "style",
                    "-webkit-touch-callout: none; -webkit-user-select: none; -khtml-user-select: none; -moz-user-select: none; -ms-user-select: none; user-select: none; "
                );
                node.setAttribute("draggable","false");
            });
        });

    }
}
