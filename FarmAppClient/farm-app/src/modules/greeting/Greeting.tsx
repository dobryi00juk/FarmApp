import React from "react"
import { Typography } from "@material-ui/core"
import styles from './Greeting.module.css'
import {url} from "inspector";
const logo = require('../../logo.png')
const graf = require('../../test.svg')


export const Greeting = () => {


  return(
        <Typography>
          <div className={styles.main}>
            <div>
                <img style={{height: '80px',float: 'left',marginRight: '15px'}} src={logo} alt=""/> Программное обеспечение управления информацией обладает функцией выявлять перспективные направления во всех сферах обработки информации и преобразовывать их в инновационные проекты.
                Для эффективной работы предприятия или организации необходимо не только собирать, хранить информацию и использовать ее для управления предприятием, но и проводить мониторинг и анализ рынка для того, чтобы планировать дальнейшее развитие организации, а также получать максимум прибыли при минимальных затратах.
                Фармацевтический рынок – один из самых насыщенных и прибыльных рынков в мире, для того, чтобы эффективно конкурировать на нем, необходимо проводить постоянный мониторинг, в чем также могут помочь информационные системы.
            </div>
              <img src={graf} style={{width: '100%',marginTop: '20px'}} alt=""/>
            <button className={styles.bubblyButton}>Click me!</button>
          </div>



        </Typography>
    )
}
