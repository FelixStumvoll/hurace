import React, { useEffect, useState } from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faAngleLeft,
    faClock,
    faLocationArrow,
    faSkiing,
    faUser,
    faFlagCheckered
} from '@fortawesome/free-solid-svg-icons';
import { Card } from '../theme/StyledComponents';
import { Race } from '../interfaces/Race';
import Axios from 'axios';
import { API_URL } from '../api';

const RacePanel = styled.div`
    display: grid;
    height: 100%;
    grid-row-gap: 10px;
    grid-template-rows: auto auto 1fr;
`;

const BackLink = styled(Link)`
    text-decoration: none;
    color: black;
    font-size: 20px;
`;

const BackIcon = styled(FontAwesomeIcon)`
    margin-right: 5px;
`;

const DetailCard = styled(Card)`
    display: flex;
    flex-direction: column;
    padding: 10px;
`;

const DetailPanel = styled.div`
    display: grid;
    grid-template-areas:
        'location discipline state'
        'gender date .';
    overflow: auto;
    max-height: 500px;
`;

interface GridAreaProps {
    gridArea: string;
}

const DescriptionHeader = styled.div`
    grid-area: descriptionHeader;
    font-weight: bold;
    margin-top: 20px;
`;

const DetailText = styled.div<GridAreaProps>`
    grid-area: ${props => props.gridArea};
    margin: auto 0px auto 0px;
    height: fit-content;
`;

const HeaderText = styled(DetailText)`
    font-size: 25px;
    font-weight: bold;
`;

const RaceDescription = styled.div`
    grid-area: description;
    word-wrap: break-word;
    overflow: auto;
`;

const DetailIcon = styled(FontAwesomeIcon)`
    margin-right: 5px;
`;

const RaceList = styled(Card)<GridAreaProps>`
    padding: 10px;
`;

export const RaceDetailView: React.FC<{ raceId: number }> = ({ raceId }) => {
    let [race, setRace] = useState<Race | undefined>(undefined);

    useEffect(() => {
        if (race !== undefined) return;
        async function loadRaceDetail() {
            let response = await Axios.get<Race>(`${API_URL}/race/${raceId}`);
            response.data.raceDate = new Date(response.data.raceDate);
            setRace(response.data);
        }
        loadRaceDetail();
    }, [race, raceId]);
    return (
        <RacePanel>
            <BackLink to="/seasons">
                <BackIcon icon={faAngleLeft} />
                <span>Zur Saisonübersicht</span>
            </BackLink>
            <DetailCard>
                <DetailPanel>
                    <HeaderText gridArea="location">
                        <DetailIcon icon={faLocationArrow} />
                        {race?.location.locationName}
                    </HeaderText>
                    <DetailText gridArea="discipline">
                        <DetailIcon icon={faSkiing} />
                        {race?.discipline.disciplineName}
                    </DetailText>
                    <DetailText gridArea="state">
                        <DetailIcon icon={faFlagCheckered} />
                        {race?.raceState.raceStateDescription}
                    </DetailText>
                    <DetailText gridArea="gender">
                        <DetailIcon icon={faUser} />
                        {race?.gender.genderDescription}
                    </DetailText>
                    <DetailText gridArea="date">
                        <DetailIcon icon={faClock} />{' '}
                        {race?.raceDate.toDateString()}
                    </DetailText>
                </DetailPanel>
                {race?.raceDescription && (
                    <>
                        <DescriptionHeader>Beschreibung:</DescriptionHeader>
                        <RaceDescription>
                            {race?.raceDescription}
                        </RaceDescription>
                    </>
                )}
            </DetailCard>
            <div>
                <RaceList gridArea="startList">Startliste</RaceList>
                <RaceList gridArea="ranking">Rangliste</RaceList>
            </div>
        </RacePanel>
    );
};
