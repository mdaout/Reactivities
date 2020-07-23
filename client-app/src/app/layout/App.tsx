import React, { useEffect, Fragment, useContext } from 'react';
// import logo from './logo.svg';
import { Container } from 'semantic-ui-react'
import NavBar from '../../features/nav/NavBar';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard';
import LoadingComponent from './LoadingComponent';
import ActivityStore from '../stores/activitystore';
import { observer } from 'mobx-react-lite';

const App = () => {
  const activityStore = useContext(ActivityStore)

  useEffect(() => {
    activityStore.localActivities();

  }, [activityStore]);

  if (activityStore.loadingInitial) return <LoadingComponent content='Loading activities...' />

  return (
    <Fragment>
      <NavBar />
      <Container style={{ marginTop: '7em' }} >
        <h1>{activityStore.title}</h1>
        <ActivityDashboard />
      </Container>

    </Fragment>
  );

}

export default observer(App);
