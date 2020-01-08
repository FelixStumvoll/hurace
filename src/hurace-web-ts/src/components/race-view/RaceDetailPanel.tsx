import React from 'react';
import styled from 'styled-components';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faClock,
    faLocationArrow,
    faSkiing,
    faUser,
    faFlagCheckered,
    faCalendarDay
} from '@fortawesome/free-solid-svg-icons';
import { getRaceDetails } from '../../common/api';
import { HeaderCard } from '../shared/HeaderCard';
import { getTime, getDate } from '../../common/timeConverter';
import { useStateAsync } from '../../hooks/useStateAsync';
import { Race } from '../../models/Race';

const DetailPanel = styled.div`
    display: flex;
`;

const InfoContent = styled.div`
    display: grid;
    grid-template-rows: repeat(5, auto);
    column-gap: 5px;
    grid-template-columns: 30px auto;
`;

const DescriptionCardWrapper = styled.div`
    margin-left: 10px;
`;

const DetailText = styled.span`
    height: fit-content;
    margin: auto 0 auto 0;
`;

const CountryText = styled(DetailText)`
    font-style: italic;
    font-weight: normal;
    font-size: 16px;
    color: ${props => props.theme.gray};
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

const DetailIcon = styled(FontAwesomeIcon)<{ fontSize: number }>`
    font-size: ${props => props.fontSize}px;
    margin: auto;
`;

export const RaceDetailPanel: React.FC<{ race: Race }> = ({ race }) => {
    return (
        <DetailPanel>
            <HeaderCard headerText="Stammdaten">
                <InfoContent>
                    <DetailIcon fontSize={25} icon={faLocationArrow} />
                    <HeaderText>
                        {race.location.locationName}
                        <CountryText>
                            {' '}
                            {race.location.country.countryName}
                        </CountryText>{' '}
                    </HeaderText>
                    <DetailIcon fontSize={16} icon={faSkiing} />
                    <DetailText>{race.discipline.disciplineName}</DetailText>
                    <DetailIcon fontSize={16} icon={faFlagCheckered} />
                    <DetailText>
                        {race.raceState.raceStateDescription}
                    </DetailText>
                    <DetailIcon fontSize={16} icon={faUser} />
                    <DetailText>{race.gender.genderDescription}</DetailText>
                    <DetailIcon fontSize={16} icon={faCalendarDay} />
                    <DetailText>{getDate(race.raceDate)}</DetailText>
                    <DetailIcon fontSize={16} icon={faClock} />
                    <DetailText>{getTime(race.raceDate)}</DetailText>
                </InfoContent>
            </HeaderCard>
            <DescriptionCardWrapper>
                <HeaderCard headerText="Beschreibung">
                    {race.raceDescription && (
                        <RaceDescription>
                            {race.raceDescription}
                        </RaceDescription>
                    )}
                </HeaderCard>
            </DescriptionCardWrapper>
        </DetailPanel>
    );
};
