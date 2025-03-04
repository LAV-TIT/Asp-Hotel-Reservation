
const menudp = document.querySelectorAll("a.nav-link");
menudp.forEach(a => {
    a.addEventListener('click', function (e) {
        e.preventDefault(); // Prevent default link behavior

        // Find the next sibling element (the .side-dropdown div)
        const dropdown = this.nextElementSibling;
        alert("ddd")
        dropdown.classList.toggle("shos")
        // Check if the next sibling is a .side-dropdown element
        //if (dropdown && dropdown.classList.contains('side-dropdown')) {
        //    // Toggle the 'show' class on the .side-dropdown div
        //    dropdown.classList.toggle('show');
        //}
    });
});

// SIDEBAR DROPDOWN

const allDropdown = document.querySelectorAll('#sidebar .side-dropdown');
const sidebar = document.getElementById('sidebar');

allDropdown.forEach(item => {
    const a = item.parentElement.querySelector('a:first-child');
    a.addEventListener('click', function (e) {
        e.preventDefault();

        // Close all other dropdowns at the same level
        const parentLi = item.parentElement;
        const parentUl = parentLi.parentElement;
        if (!this.classList.contains('active')) {
            parentUl.querySelectorAll('.side-dropdown').forEach(i => {
                if (i !== item) {
                    const aLink = i.parentElement.querySelector('a:first-child');
                    aLink.classList.remove('active');
                    i.classList.remove('show');
                }
            });
        }

        // Toggle the clicked dropdown
        this.classList.toggle('active');
        item.classList.toggle('show');

        // Recursively handle nested dropdowns
        //const nestedDropdowns = item.querySelectorAll('.side-dropdown');
        //nestedDropdowns.forEach(nestedItem => {
        //    const nestedA = nestedItem.parentElement.querySelector('a:first-child');
        //    if (this.classList.contains('active')) {
        //        nestedA.classList.add('active');
        //        nestedItem.classList.add('show');
        //    } else {
        //        nestedA.classList.remove('active');
        //        nestedItem.classList.remove('show');
        //    }
        //});
    });
});

// SIDEBAR COLLAPSE
const toggleSidebar = document.querySelector('nav .toggle-sidebar');
const allSideDivider = document.querySelectorAll('#sidebar .divider');
const clbtn = document.querySelector(".btn_side_close");

clbtn.addEventListener('click', function () {
    sidebar.classList.remove('hide');
    toggleSidebar.classList.remove('hide');
    allSideDivider.forEach(item => {
        item.classList.remove('hide');
    });
});

if (sidebar.classList.contains('hide')) {
    allSideDivider.forEach(item => {
        item.textContent = '-';
    });
    allDropdown.forEach(item => {
        const a = item.parentElement.querySelector('a:first-child');
        a.classList.remove('active');
        item.classList.remove('show');
    });
} else {
    allSideDivider.forEach(item => {
        item.textContent = item.dataset.text;
    });
}

toggleSidebar.addEventListener('click', function () {
    sidebar.classList.toggle('hide');

    if (sidebar.classList.contains('hide')) {
        allSideDivider.forEach(item => {
            item.textContent = '--';
        });

        allDropdown.forEach(item => {
            const a = item.parentElement.querySelector('a:first-child');
            a.classList.remove('active');
            item.classList.remove('show');
        });
    } else {
        allSideDivider.forEach(item => {
            item.textContent = item.dataset.text;
        });
    }
});

sidebar.addEventListener('mouseleave', function () {
    if (this.classList.contains('hide')) {
        allDropdown.forEach(item => {
            const a = item.parentElement.querySelector('a:first-child');
            a.classList.remove('active');
            item.classList.remove('show');
        });
        allSideDivider.forEach(item => {
            item.textContent = '--';
        });
    }
});

sidebar.addEventListener('mouseenter', function () {
    if (this.classList.contains('hide')) {
        allDropdown.forEach(item => {
            const a = item.parentElement.querySelector('a:first-child');
            a.classList.remove('active');
            item.classList.remove('show');
        });
        allSideDivider.forEach(item => {
            item.textContent = item.dataset.text;
        });
    }
});

//=====================

// PROFILE DROPDOWN
const profile = document.querySelector('nav .profile');
const imgProfile = profile.querySelector('img');
const dropdownProfile = profile.querySelector('.profile-link');

imgProfile.addEventListener('click', function () {
    dropdownProfile.classList.toggle('show');
})


// MENU
const allMenu = document.querySelectorAll('.content-data .head .menu');

allMenu.forEach(item => {
    const icon = item.querySelector('.icon');
    const menuLink = item.querySelector('.menu-link');

    icon.addEventListener('click', function () {
        menuLink.classList.toggle('show');
    })
})

window.addEventListener('click', function (e) {
    if (e.target !== imgProfile) {
        if (e.target !== dropdownProfile) {
            if (dropdownProfile.classList.contains('show')) {
                dropdownProfile.classList.remove('show');
            }
        }
    }

    allMenu.forEach(item => {
        const icon = item.querySelector('.icon');
        const menuLink = item.querySelector('.menu-link');

        if (e.target !== icon) {
            if (e.target !== menuLink) {
                if (menuLink.classList.contains('show')) {
                    menuLink.classList.remove('show')
                }
            }
        }
    })
})


// PROGRESSBAR
const allProgress = document.querySelectorAll('main .card .progress');

allProgress.forEach(item => {
    item.style.setProperty('--value', item.dataset.value)
})

// preview image
document.getElementById('photo').addEventListener('change', function (event) {
    const file = event.target.files[0]; // Get the selected file
    const reader = new FileReader(); // Create a FileReader object

    reader.onload = function (e) {
        // Set the `src` attribute of the image to the uploaded file's data URL
        document.getElementById('imagePreview').src = e.target.result;
    };

    // Read the file as a Data URL (base64 encoded string)
    if (file) {
        reader.readAsDataURL(file);
    }
});


