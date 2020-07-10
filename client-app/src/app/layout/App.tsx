import React, { useState, useEffect, Fragment, SyntheticEvent } from 'react';
// import logo from './logo.svg';
 
import { Container } from 'semantic-ui-react'
import { IActivity } from '../models/Activity';
import NavBar from '../../features/nav/NavBar';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard';
import agent from '../api/agent';
import LoadingComponent from './LoadingComponent';

const App = () => {

  const [activities, setActivities] = useState<IActivity[]>([])
  const [selectedActivity, setselectedActivity] = useState<IActivity | null>(null)
  const [editMode, setEditMode] = useState(false);

  const [loading,setLoading] = useState(true);

  const [submitting,setSubmitting] = useState(false);
  const [target, setTarget] = useState('');

  const handelSelectedActivity = (id: string) => {
    setselectedActivity(activities.filter(a => a.id === id)[0])
    setEditMode(false);  // when view button clicked turn edit off
  }

  const handleOpenCreateForm = () => {
    setselectedActivity(null);
    setEditMode(true);
  }

  // ... is a spreader it creates a new array. below creates an array an adds ourcnew activity to it
  const handelCreateActivity = (activity: IActivity) => {
    setSubmitting(true);
    agent.Activities.create(activity).then(() => {
      setActivities([...activities, activity])
      setselectedActivity(activity); // set detial to disply our newly created activity
      setEditMode(false);  // trun off edit mode

    }).then(() =>  setSubmitting(false))
  }
  // below creates an array of all the activites not equaling activit in quetio then addsa our activit to it
  const handelEditActivity = (activity: IActivity) => {
    setSubmitting(true);
    agent.Activities.update(activity).then(() => {
      setActivities([...activities.filter(a => a.id !== activity.id), activity])
      setselectedActivity(activity); // set detial to disply our newly created activity
      setEditMode(false);  // trun off edit mode
    }).then(() =>  setSubmitting(false))
  }
  const handleDeleteActivity = (event: SyntheticEvent<HTMLButtonElement>, id: string) => {
    setSubmitting(true);
    setTarget(event.currentTarget.name)
    agent.Activities.delete(id).then(() => {
      setActivities([...activities.filter(a => a.id !== id)])
    }).then(() =>  setSubmitting(false))

  }
  useEffect(() => {
    // axios
    //   .get<IActivity[]>('http://localhost:5000/api/activities')
    agent.Activities.list()
      .then((response) => {
        //  console.log(response);
       // let activites: IActivity[] = [];
        response.forEach(activity => {
          activity.date = activity.date.split('.')[0];
          activities.push(activity);
        })
        setActivities(response)
        // setActivities(activities)
      }).then(() => setLoading(false));
  }, []);

  if(loading) return <LoadingComponent content='Loading activities...'/>

  return (
    <Fragment>
      <NavBar openCreateForm={handleOpenCreateForm} />
      <Container style={{ marginTop: '7em' }} >
        <ActivityDashboard activities={activities}
          selectActivity={handelSelectedActivity}
          selectedActivity={selectedActivity}
          editMode={editMode}
          setEditMode={setEditMode}
          setselectedActivity={setselectedActivity}
          createActivity={handelCreateActivity}
          editActivity={handelEditActivity}
          deleteActivity={handleDeleteActivity}
          submitting={submitting}
          target={target}
        />
      </Container>

    </Fragment>
  );

}

export default App;
