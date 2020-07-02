import React, { Component } from 'react';
// import logo from './logo.svg';
import axios from 'axios';
import { Header, Icon, List } from 'semantic-ui-react'

class App extends Component {

  state = {   activities : []  }

  componentDidMount() {
    axios.get('http://localhost:5000/api/activities')
      .then((response) => {
      //  console.log(response);
        this.setState({
          activities : response.data
        })
      })
    // this.setState({
    //   values: [    //     {   "id": 1,"name": "value 101" },    //     {   "id": 2, "name": "value 102" },    //     {   "id": 3, "name": "value 103"}    //   ]
    // })
  }
  render() {
    return (
      <div>
        <Header as='h2'>
          <Icon name='users' />
          <Header.Content>Reactivities</Header.Content>
        </Header>
        <List>
        {this.state.activities.map((activity : any) => (
           // <li key={value.id}>{value.id + "-- " + value.name}</li>
            <List.Item key={activity.id}>{activity.title }</List.Item>
          ))}
        </List>
         </div>
    );
  }
}

export default App;
