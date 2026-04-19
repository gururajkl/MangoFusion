import { ToastContainer } from "react-toastify";
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
      <ToastContainer
        position="top-right"
        autoClose={4000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick={false}
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
        theme="light"
      />
    </div>
  );
}

export default App;
