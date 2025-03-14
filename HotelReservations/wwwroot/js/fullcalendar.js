document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
        timeZone: 'GMT+7',
        initialView: 'dayGridMonth',
        editable: false,
        droppable: true,
        selectable: true,
        selectMirror: true,
        select: function (info) {
            openModal(null, info.start, info.end); // Open modal with pre-filled dates
            calendar.unselect(); // Clear date selection
        },
        headerToolbar: {
            right: 'prev,next today',
            center: 'title',
            left: 'dayGridMonth addEventButton'
        },
        customButtons: {
            addEventButton: {
                text: 'Booking',
                click: function () {
                    openModal(); // Open modal without pre-filled dates
                }
            }
        },
        eventClick: function (info) {
            var customerName = info.event.extendedProps.customerName;
            var start = info.event.start;
            var end = info.event.end;
            openModal(customerName, start, end); // Open modal with customer name and dates
            info.el.style.backgroundColor = '#ff9f89';
            info.el.style.color = '#ff9f89';
            info.el.style.border = '1px solid #ff9f89';
        },
        eventDrop: function (info) {
            alert('Event moved to: ' + info.event.start.toISOString());
        },
        eventResize: function (info) {
            alert('Event resized to: ' + info.event.end.toISOString());
        },
        drop: function (info) {
            var eventData = JSON.parse(info.draggedEl.getAttribute('data-event'));
            eventData.start = info.dateStr;
            calendar.addEvent(eventData);
        }
    });

    calendar.render();

    function openModal(customerName, start, end) {
        var modal = new bootstrap.Modal(document.getElementById('eventModal'));

        // Pre-fill the form if customer name and dates are provided
        if (customerName) {
            var selectElement = document.getElementById('CustomerId');
            for (var i = 0; i < selectElement.options.length; i++) {
                if (selectElement.options[i].getAttribute('data-name') === customerName) {
                    selectElement.selectedIndex = i;
                    break;
                }
            }
        }

        if (start) {
            document.getElementById('CheckInDate').value = start.toISOString().slice(0, 16);
        }
        if (end) {
            document.getElementById('CheckOutDate').value = end.toISOString().slice(0, 16);
        }

        modal.show();
    }

    document.getElementById('eventForm').addEventListener('submit', function (e) {
        e.preventDefault();

        var customerId = document.getElementById('CustomerId').value;
        var selectedOption = document.getElementById('CustomerId')
            .options[document.getElementById('CustomerId').selectedIndex];
        var customerName = selectedOption.getAttribute('data-name');
        var start = document.getElementById('CheckInDate').value;
        var end = document.getElementById('CheckOutDate').value;

        if (customerId && start) {
            calendar.addEvent({
                title: customerName,
                start: start,
                end: end,
                customerId: customerId,
                customerName: customerName,
                className: 'custom-event'
            });

            var modal = bootstrap.Modal.getInstance(document.getElementById('eventModal'));
            modal.hide();
            document.getElementById('eventForm').reset();
        } else {
            alert('Please fill in the required fields.');
        }
    });
});