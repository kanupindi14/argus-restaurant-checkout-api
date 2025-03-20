const http = require("http");

let orders = {}; // To store active orders in the memory

const server = http.createServer((req, res) => {
    if (req.method === "POST" && req.url === "/checkout") {
        let body = "";

        req.on("data", chunk => {
            body += chunk.toString();
        });

        req.on("end", () => {
            const orderId = Date.now(); // Assuming Unique Order ID
            const requestData = JSON.parse(body);
            const { starters, mains, drinks, orderTime } = requestData;

            let total = (starters * 4) + (mains * 7) + (drinks * 2.50); //requirement logic

            // To apply 30% discount if drinks are ordered before 19:00
            if (orderTime && orderTime < "19:00") {
                total -= (drinks * 0.30 * 2.50);
            }

            // To apply 10% service charge on food items (starters + mains)
            total += ((starters * 4) + (mains * 7)) * 0.10;

            // Save the order
            orders[orderId] = { starters, mains, drinks, total };

            res.writeHead(200, { "Content-Type": "application/json" });
            res.end(JSON.stringify({ orderId, total }));
        });

    } else if (req.method === "PATCH" && req.url === "/modify-order") {
        let body = "";

        req.on("data", chunk => {
            body += chunk.toString();
        });

        req.on("end", () => {
            const requestData = JSON.parse(body);
            const { orderId, cancelItems } = requestData;

            if (!orders[orderId]) {
                res.writeHead(404, { "Content-Type": "application/json" });
                res.end(JSON.stringify({ error: "Order not found" }));
                return;
            }

            let order = orders[orderId];

            // To check for over-cancellation (preventing negative values)
            if (
                (cancelItems.starters && cancelItems.starters > order.starters) ||
                (cancelItems.mains && cancelItems.mains > order.mains) ||
                (cancelItems.drinks && cancelItems.drinks > order.drinks)
            ) {
                res.writeHead(400, { "Content-Type": "application/json" });
                res.end(JSON.stringify({ error: "Cannot remove more items than ordered" }));
                return;
            }

            // To deduct any canceled starters from the order
            if (cancelItems.starters && order.starters >= cancelItems.starters) {
                order.total -= (cancelItems.starters * 4) + (cancelItems.starters * 4 * 0.10);
                order.starters -= cancelItems.starters;
            }

            // To deduct any canceled mains from the order
            if (cancelItems.mains && order.mains >= cancelItems.mains) {
                order.total -= (cancelItems.mains * 7) + (cancelItems.mains * 7 * 0.10);
                order.mains -= cancelItems.mains;
            }

            // To deduct any canceled drinks from the order
            if (cancelItems.drinks && order.drinks >= cancelItems.drinks) {
                order.total -= cancelItems.drinks * 2.50;
                order.drinks -= cancelItems.drinks;
            }

            // to make sure total is correct (not negative)
            order.total = Math.max(order.total, 0);

            res.writeHead(200, { "Content-Type": "application/json" });
            res.end(JSON.stringify({ message: "Order modified", total: order.total }));
        });

    } else if (req.method === "DELETE" && req.url === "/cancel-order") {
        let body = "";

        req.on("data", chunk => {
            body += chunk.toString();
        });

        req.on("end", () => {
            const requestData = JSON.parse(body);
            const { orderId } = requestData;

            if (!orders[orderId]) {
                res.writeHead(404, { "Content-Type": "application/json" });
                res.end(JSON.stringify({ error: "Order not found" }));
                return;
            }

            // to remove the order
            delete orders[orderId];

            res.writeHead(200, { "Content-Type": "application/json" });
            res.end(JSON.stringify({ message: "Order canceled", total: 0 }));
        });

    } else {
        res.writeHead(404, { "Content-Type": "application/json" });
        res.end(JSON.stringify({ error: "Invalid endpoint or request" }));
    }
});

server.listen(5000, () => console.log("Mock restaurent checkout API Running on Port 5000"));
