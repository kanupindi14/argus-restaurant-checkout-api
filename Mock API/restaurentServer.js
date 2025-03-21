const http = require("http");

let orders = {}; // To store active orders in memory

const server = http.createServer((req, res) => {
  if (req.method === "POST" && req.url === "/checkout") {

    let body = "";

    req.on("data", (chunk) => {
      body += chunk.toString();
    });

    req.on("end", () => {
      const orderId = `${Date.now()}_${Math.floor(Math.random() * 1000)}`;
      const requestData = JSON.parse(body);
      const { starters = 0, mains = 0, drinks = 0, orderTime } = requestData;

      if (
        typeof starters !== "number" ||
        typeof mains !== "number" ||
        typeof drinks !== "number"
      ) {
        res.writeHead(400, { "Content-Type": "application/json" });
        return res.end(JSON.stringify({ error: "Missing or invalid input" }));
      }

      // Reject negative quantities
      if (starters < 0 || mains < 0 || drinks < 0) {
        res.writeHead(400, { "Content-Type": "application/json" });
        res.end(JSON.stringify({ error: "Invalid quantity" }));
        return;
      }

      // Reject zero quantity order
      if (starters === 0 && mains === 0 && drinks === 0) {
        res.writeHead(400, { "Content-Type": "application/json" });
        res.end(JSON.stringify({ error: "Invalid order" }));
        return;
      }

      let total = starters * 4 + mains * 7 + drinks * 2.5;

      // To apply 30% discount if drinks are ordered before 19:00
      if (orderTime) {
        const [hour] = orderTime.split(":").map(Number);
        if (hour < 19) {
          total -= drinks * 0.3 * 2.5;
        }
      }

      // To apply 10% service charge on food items (starters + mains)
      total += (starters * 4 + mains * 7) * 0.1;

      // Save the order
      orders[orderId] = { starters, mains, drinks, total };

      res.writeHead(200, { "Content-Type": "application/json" });
      res.end(JSON.stringify({ orderId, total }));
    });
  } else if (req.method === "PATCH" && req.url === "/modify-order") {
    let body = "";

    req.on("data", (chunk) => {
      body += chunk.toString();
    });

    req.on("end", () => {
      const requestData = JSON.parse(body);
      const { orderId, cancelItems } = requestData;

      if (!orders[orderId]) {
        res.writeHead(400, { "Content-Type": "application/json" });
        return res.end(
          JSON.stringify({ error: "Order not found or already canceled" })
        );
      }

      if (!orders[orderId]) {
        res.writeHead(404, { "Content-Type": "application/json" });
        res.end(
          JSON.stringify({ error: "Order not found or already canceled" })
        );
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
        return res.end(
          JSON.stringify({ error: "Cannot remove more items than ordered" })
        );
      }

      // To deduct any canceled starters from the order
      if (cancelItems.starters) {
        order.total -=
          cancelItems.starters * 4 + cancelItems.starters * 4 * 0.1;
        order.starters -= cancelItems.starters;
      }

      // To deduct any canceled starters from the order
      if (cancelItems.mains) {
        order.total -= cancelItems.mains * 7 + cancelItems.mains * 7 * 0.1;
        order.mains -= cancelItems.mains;
      }

      // To deduct any canceled mains from the order
      if (cancelItems.drinks) {
        order.total -= cancelItems.drinks * 2.5;
        order.drinks -= cancelItems.drinks;
      }

      // to make sure total is correct (not negative)
      order.total = Math.max(order.total, 0);

      res.writeHead(200, { "Content-Type": "application/json" });
      res.end(
        JSON.stringify({ message: "Order modified", total: order.total })
      );
    });
  } else if (req.method === "DELETE" && req.url === "/cancel-order") {
    let body = "";

    req.on("data", (chunk) => {
      body += chunk.toString();
    });

    req.on("end", () => {
      const requestData = JSON.parse(body);
      const { orderId } = requestData;

      if (!orders[orderId]) {
        res.writeHead(404, { "Content-Type": "application/json" });
        return res.end(JSON.stringify({ error: "Order not found" }));
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

server.listen(5000, () =>
  console.log("Mock restaurant checkout API Running on Port 5000")
);
