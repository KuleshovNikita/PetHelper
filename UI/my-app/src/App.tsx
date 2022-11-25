import { BrowserRouter } from 'react-router-dom';
import React, { useEffect } from 'react';
import { useStore } from './api/stores/Store';
import { observer } from "mobx-react-lite";
import LoadingComponent from './components/Loading/LoadingComponent';
import 'react-toastify/dist/ReactToastify.css';
import { AppRoutes } from './components/Routing/Routes';
import { ToastContainer } from "react-toastify";

function App() {
  const { commonStore, userStore } = useStore();

  useEffect(() => {
    userStore.getCurrentUser().finally(() => commonStore.setAppLoaded());
  }, [commonStore, userStore]);

  if (!commonStore.appLoaded) {
    return <LoadingComponent />;
  }

  return (
    <BrowserRouter>
      <React.StrictMode>
        <main className="main">
          <AppRoutes />
        </main>

        <ToastContainer position={"bottom-right"} hideProgressBar={true}/>
      </React.StrictMode>
    </BrowserRouter>
  );
}

export default observer(App);
