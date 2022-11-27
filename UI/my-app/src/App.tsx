import { BrowserRouter } from 'react-router-dom';
import React, { useEffect } from 'react';
import { useStore } from './api/stores/Store';
import { observer } from "mobx-react-lite";
import LoadingComponent from './components/Loading/LoadingComponent';
import 'react-toastify/dist/ReactToastify.css';
import { AppRoutes } from './components/Routing/Routes';
import { ToastContainer } from "react-toastify";
import AppLoader from './AppLoader';
import Header from './components/Header/Header';

function App() {
  const { commonStore, userStore, petStore } = useStore();

  useEffect(() => {
    AppLoader({ commonStore, userStore, petStore });
  }, [commonStore, userStore, petStore]);

  if (!commonStore.appLoaded) {
    return <LoadingComponent />;
  }

  return (
    <BrowserRouter>
      <React.StrictMode>
        <Header />        
        <main className="main">
          <AppRoutes />
        </main>

        <ToastContainer position={"bottom-right"} hideProgressBar={true}/>
      </React.StrictMode>
    </BrowserRouter>
  );
}

export default observer(App);
