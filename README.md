Transformer Monitor System
This is a simple dashboard for monitoring electric transformers in real-time. It helps users track voltage levels, check asset health, and create work orders if something go wrong.

How to use it?
Top Header Button: Use the big "+" button in the main header to add a new monitor card to your screen. You can add as many as you want.

Monitor Configration: Each card has its own gear icon. Click it to open settings for that specific monitor.

Card Settings: Inside settings, you can filter assets by region or health status (e.g., show only Critical units). You can also toggle the chart on or off.

Remove Monitor: If you want to delete a card, use the delete option inside the card settings.

Quick View: Click on any transformer name to see the full details and chart befor you dispatch a team.

Dispatch Team: Use the "Create Work Order" button to send someone to the field. Pick a prioirty level and add a note.

Alerts: Check the top bar for notifications that show up immedaitely when a transformer fails.

Tech Stack
Vue 3: For building the user interrface.

Pinia: To manage the state of the app (UI, Monitors, and Transformers).

TypeScript: To make the code safer and reduce buggs.

ECharts: For the responsive voltage charts.

CSS Variables: To handle the theme switching easly.

Project Structure
src/application: Holds our Pinia stores and logic.

src/domain: Definitions of our data like Transformrs and Cards.

src/presentation: All the Vue components and styles.

src/data: Services for handeling data and alerts.

How to run it
Clone the repo to your machin.

Run npm install to get the packages.

Run npm run dev to start the server.

Open your browser and go to localhost:5173.

Some notes
The app is fuly responsive. The teme preference is saved in your browser, so it stays the same even if you refreh the page.
