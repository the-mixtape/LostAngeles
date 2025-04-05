$(document).ready(function () {
    const content = document.getElementById("Inventory");
    if (!content) return;

    let InventoryMouse = false;
    content.addEventListener('mouseenter', () => InventoryMouse = true)
    content.addEventListener('mouseleave', () => InventoryMouse = false)

    function DisplayInventory(show) {
        if (show) {
            $('#Inventory').fadeIn(200);
        } else {
            $('#Inventory').fadeOut(200);
        }
    }

    DisplayInventory(false);

    window.addEventListener('message', function (event) {
        const item = event.data;
        if (item?.Type === "Inventory") {
            DisplayInventory(item.Display === true);
        }
    });

    $(document).on('keydown', function (event) {
        const key = event.which;
        if (InventoryMouse && (key === 27 || key === 9 || key === 73)) {
            $.post('https://lostangeles/Inventory::Close', JSON.stringify({}));
        }
    });
});