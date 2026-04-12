import Footer from "./components/layouts/Footer";
import Header from "./components/layouts/Header";
import ApplicationRoutes from "./components/routes/ApplicationRoutes";

function App() {
  return (
    <div className="d-flex flex-column min-vh-100 bg-body">
      <Header />
      <main className="flex-grow-1">
        <ApplicationRoutes />
      </main>
      <Footer />
    </div>
  );
}

export default App;
