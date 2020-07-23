import { computed, observable, action, configure, runInAction } from 'mobx';
import { IActivity } from '../models/Activity';
import LoadingComponent from '../layout/LoadingComponent';
import agent from '../api/agent';
import { createContext, SyntheticEvent } from 'react';

configure({ enforceActions: 'always' });

class ActivityStore {
  @observable activityRegistry = new Map();    // replacing activities
  @observable activities: IActivity[] = [];   // in process of being retired from this app an replaced with Map()
  @observable selectedActivity: IActivity | undefined;
  @observable loadingInitial = false;
  @observable editMode = false;
  @observable submitting = false;
  @observable target = '';

  @observable title = "Hello From MOBX";

  @computed get ActivitiesByDate() {
    // return this.activities.sort((a, b) => Date.parse(a.date) - Date.parse(b.date)); // 
    // From when we were using a list.  Map is not an Array so need to convert to array for sort
    return Array.from(this.activityRegistry.values()).sort(
      (a, b) => Date.parse(a.date) - Date.parse(b.date));   // Now using Maps
  }

  @action localActivities = async () => {
    this.loadingInitial = true;
    try {
      const activities = await agent.Activities.list();
      runInAction('loading activities', () => {
        activities.forEach((activity) => {
          activity.date = activity.date.split('.')[0];
          //  this.activities.push(activity);  from when we were using a list
          this.activityRegistry.set(activity.id, activity);  // Now using Maps
        });
        this.loadingInitial = false;
      })

    } catch (error) {
      runInAction('load activity error', () => {
        this.loadingInitial = false;
      });
      console.log(error);
    }
  }
  @action creatActivity = async (activity: IActivity) => {
    this.submitting = true;
    try {
      await agent.Activities.list();
      runInAction('creating activity', () => {
        // this.activities.push(activity);   // from when we were using a list
        this.activityRegistry.set(activity.id, activity);  // Now using Maps
        this.editMode = false;
        this.submitting = false;
      });

    } catch (error) {
      runInAction('create activity error', () => {
        this.submitting = false;
      });
      console.log(error);
    }
  }

  @action editActivity = async (activity: IActivity) => {
    this.submitting = true;
    try {
      await agent.Activities.update(activity);
      runInAction('editing activity', () => {
        this.activityRegistry.set(activity.id, activity);
        this.selectedActivity = activity;
        this.editMode = false;
        this.submitting = false;
      });
    } catch (error) {
      runInAction('edit activity error', () => {
        this.submitting = false;
      });
      console.log(error);
    }
  }

  @action openCreateForm = () => {
    this.editMode = true;
    this.selectedActivity = undefined;

  }
  @action openEditForm = (id: string) => {
    this.selectedActivity = this.activityRegistry.get(id);
    this.editMode = true;
  }
  @action deleteActivity = async (event: SyntheticEvent<HTMLButtonElement>, id: string) => {
    this.submitting = true;
    this.target = event.currentTarget.name;
    try {
      await agent.Activities.delete(id);
      runInAction('deleting activity', () => {
        this.activityRegistry.delete(id);
        this.submitting = false;
        this.target = '';
      });
    } catch (error) {
      runInAction('delete activity error', () => {
        this.submitting = false;
        this.target = '';
      });
      console.log(error);
    }
  }

  @action cancelSelectedActivity = () => {
    this.selectedActivity = undefined;
  }
  @action cancelFormOpen = () => {
    this.editMode = false;
  }
  @action selectActivity = (id: string) => {
    // this.selectedActivity = this.activities.find(a => a.id === id);  // from when we were using a list
    this.selectedActivity = this.activityRegistry.get(id);    // Now using Maps
    this.editMode = false;
  }
}
export default createContext(new ActivityStore())